
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
using Application.Constants;
using Infrastructure.Abstractions;
using System.Text.Json;
using Application.Helpers;


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

        public async Task<Result<ApiResponse<Order>>> HandleInitiatePaypallOrder(string userId)
        {
            var userOrder = await _orderRepository.FindOrderByUserId(userId);
            var errorMessage = "";
            if (userOrder != null)
            {
                var printFullOrderResult = await GetPrintfullOrder(userOrder.PrintfullOrderId);
                if (printFullOrderResult.IsSuccess)
                {
                    var printfullItemsPrice = printFullOrderResult.Value.Result.Costs.Subtotal;
                    var shippingPrice = double.Parse(printFullOrderResult.Value.Result.Costs.Shipping);
                    var myItemPrice = PriceCalculator.CalculatePrice(double.Parse(printfullItemsPrice));
                    double totalPrice = myItemPrice + shippingPrice;
                    var createPaypallOrderResult = await CreatePaypallOrder(totalPrice.ToString());
                    if (IsPaypallOrderRequestSuccess(createPaypallOrderResult))
                    {
                        userOrder.PaypallOrderId = createPaypallOrderResult.Data.Id;
                        _orderRepository.SaveChanges();
                        return Result<ApiResponse<Order>>.Success(createPaypallOrderResult);
                    }
                }
            }

            return Result<ApiResponse<Order>>.Failure(new Error(errorMessage));
        }

        public async Task<ApiResponse<Order>> HandleCapturePaypallOrder(string paypallOrderId)
        {
            var capturePaypallRequestResult = await CapturePaypallOrder(paypallOrderId);
            //confirm printfull order is paypall is captured
            return capturePaypallRequestResult;
        }

        public async Task<Result<CostCalculation>> CalculateTotalCost(CheckoutRequest checkoutRequest, string userId)
        {
            var checkout = new CheckoutRequest();
            checkout.CartItems = checkoutRequest.CartItems;
            checkout.Recipient = checkoutRequest.Recipient;

            var result = await CreatePrintfullOrder(checkoutRequest);

            if (result.IsSuccess)
            {
                var costCalculation = new CostCalculation();
                costCalculation.TotalCost = result.Value.Result.Costs.Total;
                costCalculation.ItemsCost = result.Value.Result.Costs.Subtotal;
                costCalculation.ShippingCost = result.Value.Result.Costs.Shipping;

                var orderId = await _orderRepository.CreateOrder(new Domain.Entities.Order()
                {
                    UserId = userId,
                    PrintfullOrderId = result.Value.Result.Id
                });

                if (orderId > 0)
                {
                    return Result<CostCalculation>.Success(costCalculation);
                }
                else
                {
                    CancelPrintfullOrder(result.Value.Result.Id);
                    return Result<CostCalculation>.Failure(new Error("Internal server error"));
                }
                
            }
            else
            {
                return Result<CostCalculation>.Failure(new Error(result.Error.Message));
            }
            
        }

        private async Task<ApiResponse<Order>> CreatePaypallOrder(string amount)
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
                ApiResponse<Order> result = await _ordersController.OrdersCreateAsync(ordersCreateInput);
                return result;
            }
            catch (Exception ex) 
            {

            }

            return new ApiResponse<Order>(500,new Dictionary<string, string>(), new Order());
        }

        private bool IsPaypallOrderRequestSuccess(ApiResponse<Order> order)
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

        private async Task<dynamic> CapturePaypallOrder(string paypallOrderID)
        {
            OrdersCaptureInput ordersCaptureInput = new OrdersCaptureInput
            {
                Id = paypallOrderID,
            };

            ApiResponse<Order> result = await _ordersController.OrdersCaptureAsync(ordersCaptureInput);

            return result;
        }


        private async Task<Result<PrintfullOrderResponse>> CreatePrintfullOrder(CheckoutRequest checkoutRequest)
        {
            var client = _httpClientFactory.CreateClient("printfull");
            var orderBody = await CreateOrderBodyForRequest(checkoutRequest);
            var result = new HttpResponseMessage();

            try
            {
                result = await client.PostAsJsonAsync<CreatePrintfullOrderRequest>("/orders", orderBody);
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode) 
                {
                    return Result<PrintfullOrderResponse>.Success(JsonSerializer.Deserialize<PrintfullOrderResponse>(content));
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

        private async Task<CreatePrintfullOrderRequest> CreateOrderBodyForRequest(CheckoutRequest checkoutRequest)
        {
            var orderBody = new CreatePrintfullOrderRequest();
            var recipient = new PrintfullOrderRecipient()
            {
                Name = checkoutRequest.Recipient!.FirstName + " " + checkoutRequest.Recipient.LastName,
                Address1 = checkoutRequest.Recipient!.Address!,
                City = checkoutRequest.Recipient!.City!,
                CountryCode = checkoutRequest.Recipient!.Country!,
                CountryName = "Serbia",
                Zip = checkoutRequest.Recipient!.Zip!,
                Phone = checkoutRequest.Recipient!.Phone!,
                Email = checkoutRequest.Recipient!.Email!
            };

            var items = new List<PrintfullOrderItem>();

            foreach (var i in checkoutRequest.CartItems)
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
    }
}
