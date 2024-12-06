
using Infrastracture.Interfaces.IServices;
using Microsoft.Extensions.Options;
using PaypalServerSdk.Standard.Authentication;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Controllers;
using PaypalServerSdk.Standard.Models;
using Microsoft.Extensions.Logging;
using Infrastracture.Models;
using PaypalServerSdk.Standard.Http.Response;
using Infrastracture.Interfaces.IRepositories;
using System.Net.Http.Json;
using Infrastructure.Abstractions.Errors;
using Infrastructure.Abstractions;
using System.Text.Json;
using Application.Helpers;
using Infrastracture.Abstractions;
using Domain.Entities;
using Order = PaypalServerSdk.Standard.Models.Order;
using System.Diagnostics.Metrics;



namespace Application.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOrderRepository _orderRepository;
        private readonly IDesignRepository _designRepository;
        private readonly OrdersController _ordersController;
        private readonly PaymentsController _paymentsController;
        private readonly Dictionary<string, CheckoutPaymentIntent> _paymentIntentMap;
        private string _paypalClientId;
        private string _paypalClientSecret;

        public CheckoutService(IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory, IOrderRepository orderRepository, IDesignRepository designRepository)
        {
            _httpClientFactory = httpClientFactory;
            _orderRepository = orderRepository;
            _paypalClientId = appSettings.Value!.PAYPAL_CLIENT_ID!;
            _paypalClientSecret = appSettings.Value!.PAYPAL_CLIENT_SECRET!;
            _paymentIntentMap = new Dictionary<string, CheckoutPaymentIntent> {
              {
                "CAPTURE",
                CheckoutPaymentIntent.Capture
              },
              {
                "AUTHORIZE",
                CheckoutPaymentIntent.Authorize
              }
            };

            PaypalServerSdkClient client = new PaypalServerSdkClient.Builder()
              .Environment(PaypalServerSdk.Standard.Environment.Sandbox)
              .ClientCredentialsAuth(
                new ClientCredentialsAuthModel.Builder(_paypalClientId, _paypalClientSecret).Build()
              )
              .LoggingConfig(config =>
                config
                .LogLevel(LogLevel.Information)
                .RequestConfig(reqConfig => reqConfig.Body(true))
                .ResponseConfig(respConfig => respConfig.Headers(true))
              )
              .Build();

            _ordersController = client.OrdersController;
            _paymentsController = client.PaymentsController;
            _designRepository = designRepository;
        }

        public async Task<Result<ApiResponse<PaypalServerSdk.Standard.Models.Order>>> HandleInitiatePaypallOrder(string userId)
        {
            var userOrder = await _orderRepository.FindOrderByUserId(userId, true);
            if (userOrder != null)
            {
                var createPaypallOrderResult = await CreatePaypallOrder(userOrder.TotalCost.ToString());
                if (IsPaypallOrderRequestSuccess(createPaypallOrderResult))
                {
                    userOrder.PaypallOrderId = createPaypallOrderResult.Data.Id;
                    _orderRepository.SaveChanges();
                    return Result<ApiResponse<Order>>.Success(createPaypallOrderResult);
                }
                else
                {
                    //see if delete here
                    var deleteResult = _orderRepository.DeleteOrder(userOrder);
                    _orderRepository.SaveChanges();
                }
            }

            return Result<ApiResponse<Order>>.Failure(new Error("Something went wrong with order processing"));
        }

        public async Task<Result<string>> HandleCapturePaypallOrder(string paypallOrderId, string userId)
        {
            var capturePaypallRequestResult = await CapturePaypallOrder(paypallOrderId);
            if (capturePaypallRequestResult.IsSuccess)
            {
                var userOrder = await _orderRepository.FindOrderByUserId(userId, true);

                if (userOrder != null) 
                {
                    var createPrintfullOrderResult = await CreatePrintfullOrder(userOrder);

                    if (createPrintfullOrderResult.IsSuccess)
                    {
                        userOrder.PrintfullOrderId = createPrintfullOrderResult.Value!.Result.Id;
                        userOrder.PaypallCaptureId = capturePaypallRequestResult.Value;
                        long printfullOrderId = userOrder.PrintfullOrderId;
                        userOrder.Current = false;
                        _orderRepository.SaveChanges();
                        return Result<string>.Success( "Payment was success, you can track your order...");
                    }
                }
                else
                {
                   
                }
            }
            else
            {
                
            }

            return Result<string>.Failure(new Error("Paypall failed"));
        }

        public async Task<Result<CostCalculation>> EstimateTotalCost(CheckoutRequest checkoutRequest, string userId)
        {
            var checkout = new CheckoutRequest();
            checkout.CartItems = checkoutRequest.CartItems;
            checkout.Recipient = checkoutRequest.Recipient;

            var result = await EstimatePrintfullOrderCosts(checkoutRequest);

            if (result.IsSuccess)
            {
                var costCalculation = new CostCalculation();
                costCalculation.ItemsCost = PriceCalculator.AddKaymakToItemPrice(result.Value!.Result.Costs.Subtotal);
                costCalculation.ShippingCost = PriceCalculator.AddKaymakToShiping(result.Value.Result.Costs.Shipping);
                costCalculation.TotalCost = costCalculation.ItemsCost + costCalculation.ShippingCost;
                var orderItems = new List<OrderItem>();

                checkout.CartItems.ForEach(cartItem => 
                {
                    var orderItem = new OrderItem();
                    orderItem.ProductId = cartItem.ProductId;
                    orderItem.Quantity = cartItem.Quantity;
                    orderItem.DesignId = cartItem.DesignId;

                    orderItems.Add(orderItem);
                });

                var userOrder = await _orderRepository.FindOrderByUserId(userId, true);

                if (userOrder == null)
                {
                    var orderId = await _orderRepository.CreateOrder(new Domain.Entities.Order()
                    {
                        UserId = userId,
                        OrderItems = orderItems,
                        Current = true,
                        TotalCost = costCalculation.TotalCost,
                        Recipient = new Recipient 
                        { 
                            Address = checkoutRequest.Recipient!.Address!,
                            Email = checkoutRequest.Recipient.Email!,
                            Phone = checkoutRequest.Recipient.Phone!,
                            FirstName = checkoutRequest.Recipient.FirstName!,
                            LastName = checkoutRequest.Recipient.LastName!,
                            City = checkoutRequest.Recipient.City!,
                            Country = checkoutRequest.Recipient.Country!,
                            Zip = checkoutRequest.Recipient.Zip!
                        }
                    });
                }
                else
                {
                    userOrder.OrderItems = orderItems;
                    userOrder.TotalCost = costCalculation.TotalCost;
                    userOrder.Recipient.Address = checkoutRequest.Recipient!.Address!;
                    userOrder.Recipient.Email = checkoutRequest.Recipient.Email!;
                    userOrder.Recipient.Phone = checkoutRequest.Recipient.Phone!;
                    userOrder.Recipient.FirstName = checkoutRequest.Recipient.FirstName!;
                    userOrder.Recipient.LastName = checkoutRequest.Recipient.LastName!;
                    userOrder.Recipient.City = checkoutRequest.Recipient.City!;
                    userOrder.Recipient.Country = checkoutRequest.Recipient.Country!;
                    userOrder.Recipient.Zip = checkoutRequest.Recipient.Zip!;
                    _orderRepository.SaveChanges();
                }
                
                return Result<CostCalculation>.Success(costCalculation);
            }
            
            return Result<CostCalculation>.Failure(new Error("Order costs could not be estimated at this moment"));
        }

        private async Task<ApiResponse<PaypalServerSdk.Standard.Models.Order>> CreatePaypallOrder(string amount)
        {
            OrdersCreateInput ordersCreateInput = new OrdersCreateInput
            {
                Body = new OrderRequest
                {
                    Intent = _paymentIntentMap["CAPTURE"],
                    PurchaseUnits = new List<PurchaseUnitRequest> 
                    {
                        new PurchaseUnitRequest 
                        {
                            Amount = new AmountWithBreakdown 
                            {
                                CurrencyCode = "USD", MValue = amount,
                            },
                        },
                    },
                },
            };

            try
            {
                ApiResponse<PaypalServerSdk.Standard.Models.Order> result = await _ordersController.OrdersCreateAsync(ordersCreateInput);
                return result;
            }
            catch (Exception ex) 
            {

            }

            return new ApiResponse<PaypalServerSdk.Standard.Models.Order>(500,new Dictionary<string, string>(), new PaypalServerSdk.Standard.Models.Order());
        }

        private bool IsPaypallOrderRequestSuccess(ApiResponse<PaypalServerSdk.Standard.Models.Order> order)
        {
            if (order.StatusCode is 201)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<Result<string>> CapturePaypallOrder(string paypallOrderID)
        {
            OrdersCaptureInput ordersCaptureInput = new OrdersCaptureInput
            {
                Id = paypallOrderID,
            };

            ApiResponse<PaypalServerSdk.Standard.Models.Order> result = await _ordersController.OrdersCaptureAsync(ordersCaptureInput);

            if (result.StatusCode is 201)
            {
                return Result<string>.Success(result.Data.PurchaseUnits[0].Payments.Captures[0].Id);
            }
            else
            {
                return Result<string>.Failure(new Error("Failed to capture paypall order"));
            }
          
        }

        private async Task<ApiResponse<Refund>> RefundCapturedPayment(string captureId)
        {
            CapturesRefundInput capturesRefundInput = new CapturesRefundInput
            {
                CaptureId = captureId,
            };

            try
            {
                ApiResponse<Refund> result = await _paymentsController.CapturesRefundAsync(capturesRefundInput);
                return result;
            }
            catch (Exception ex)
            {

            }

            return new ApiResponse<Refund>(500, new Dictionary<string, string>(), new Refund());
        }

        private async Task<Result<EstimatePrintfullOrderCosts>> EstimatePrintfullOrderCosts(CheckoutRequest checkoutRequest)
        {
            var client = _httpClientFactory.CreateClient("printfull");
            var userOrderModel = MapCheckoutRequestToOrder(checkoutRequest);
            var orderBody = await CreateOrderBodyForRequest(userOrderModel);
            var result = new HttpResponseMessage();

            try
            {
                result = await client.PostAsJsonAsync<CreatePrintfullOrderRequest>("/orders/estimate-costs", orderBody);
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    return Result<EstimatePrintfullOrderCosts>.Success(JsonSerializer.Deserialize<EstimatePrintfullOrderCosts>(content));
                }
                else
                {
                    var error = JsonSerializer.Deserialize<ErrorResponsePrintfull>(content);
                }

            }
            catch (Exception ex)
            {
                //error message from printfull api resposnse for logs
            }

            return Result<EstimatePrintfullOrderCosts>.Failure(new Error("Could not process order right now"));
        }

        private async Task<Result<PrintfullOrderResponse>> CreatePrintfullOrder(Domain.Entities.Order userOrder)
        {
            var client = _httpClientFactory.CreateClient("printfull");
            var orderBody = await CreateOrderBodyForRequest(userOrder);
            var result = new HttpResponseMessage();

            try
            {
                result = await client.PostAsJsonAsync<CreatePrintfullOrderRequest>("/orders?confirm=true", orderBody);
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode) 
                {
                    return Result<PrintfullOrderResponse>.Success(JsonSerializer.Deserialize<PrintfullOrderResponse>(content));
                }
                else
                {
                    var error = JsonSerializer.Deserialize<ErrorResponsePrintfull>(content);
                }
                
            }
            catch (Exception ex)
            {
                //error message from printfull api resposnse for logs
            }

            return Result<PrintfullOrderResponse>.Failure(new Error("Could not process order right now"));
        }

        private void CancelPrintfullOrder(long orderId)
        {
            var client = _httpClientFactory.CreateClient("printfull");
            Task.Run(async () =>
            {
                try
                {
                    var result = await client.DeleteAsync($"/orders/{orderId}");
                }
                catch (Exception ex)
                {
                    
                }
            });
        }

        private async Task<Result<PrintfullOrderResponseGet>> GetPrintfullOrder(long orderId)
        {
            var client = _httpClientFactory.CreateClient("printfull");
            
            var result = new HttpResponseMessage();

            try
            {
                result = await client.GetAsync($"/orders/{orderId}");

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    return Result<PrintfullOrderResponseGet>.Success(JsonSerializer.Deserialize<PrintfullOrderResponseGet>(content));
                }

            }
            catch (Exception ex)
            {
            }

            return Result<PrintfullOrderResponseGet>.Failure(new Error("Message from response"));//error message from printfull api resposnse
        }

        private async Task<Result<Generic>> ConfirmPrintfullOrder(long orderId)
        {
            var client = _httpClientFactory.CreateClient("printfull");

            var result = new HttpResponseMessage();

            try
            {
                result = await client.PostAsync($"/orders/{orderId}/confirm", null);

                if (result.IsSuccessStatusCode)
                {
                    
                    return Result<Generic>.Success(new Generic() { Value = "Order confirmed"});
                }
                else
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var error = JsonSerializer.Deserialize<ErrorResponsePrintfull>(content);
                    return Result<Generic>.Failure(new Error("Could not confirm order"));
                }

            }
            catch (Exception ex)
            {
            }

            return Result<Generic>.Failure(new Error("Could not confirm order"));
        }

        private async Task<CreatePrintfullOrderRequest> CreateOrderBodyForRequest(Domain.Entities.Order userOrder)
        {
            var orderBody = new CreatePrintfullOrderRequest();
            var recipient = new PrintfullOrderRecipient()
            {
                Name = userOrder.Recipient!.FirstName + " " + userOrder.Recipient.LastName,
                Address1 = userOrder.Recipient!.Address!,
                City = userOrder.Recipient!.City!,
                CountryCode = userOrder.Recipient!.Country!,
                CountryName = Constants.CountriesNames.CountryNames()[userOrder.Recipient.Country],
                Zip = userOrder.Recipient!.Zip!,
                Phone = userOrder.Recipient!.Phone!,
                Email = userOrder.Recipient!.Email!
            };

            var items = new List<PrintfullOrderItem>();

            foreach (var i in userOrder.OrderItems)
            {
                var design = await _designRepository.GetDesignByIdAsync(i.DesignId);
                if (design != null && !string.IsNullOrEmpty(design.ImgForPrintUrl))
                {
                    var item = new PrintfullOrderItem()
                    {
                        Quantity = i.Quantity,
                        VariantId = Constants.Product.ProductVariants()[i.ProductId],
                        Files = new List<FileForPrinting>()
                    };

                    item.Files.Add(new FileForPrinting()
                    {
                        Url = design.ImgForPrintUrl!
                    });

                    items.Add(item);
                }
            }

            orderBody.Recipient = recipient;
            orderBody.Items = items;
            return orderBody;
        }

        private Domain.Entities.Order MapCheckoutRequestToOrder(CheckoutRequest checkout)
        {
            var order = new Domain.Entities.Order();
            order.OrderItems = new List<OrderItem>();
            checkout.CartItems.ForEach(cartItem =>
            {
                var orderItem = new OrderItem();
                orderItem.ProductId = cartItem.ProductId;
                orderItem.Quantity = cartItem.Quantity;
                orderItem.DesignId = cartItem.DesignId;

                order.OrderItems.Add(orderItem);
            });

            order.Recipient = new Recipient
            {
                Address = checkout.Recipient!.Address!,
                Email = checkout.Recipient.Email!,
                Phone = checkout.Recipient.Phone!,
                FirstName = checkout.Recipient.FirstName!,
                LastName = checkout.Recipient.LastName!,
                City = checkout.Recipient.City!,
                Country = checkout.Recipient.Country!,
                Zip = checkout.Recipient.Zip!
            };

            return order;
        }
    }
}
