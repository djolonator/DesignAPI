
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

        public async Task<ApiResponse<Order>> HandleInitiateCheckout(CheckoutRequest checkoutRequest)
        {
            //printfull order start             id
            //paypall start                     id
            // kreiraj order u bazi


            var checkout = new CheckoutRequest();
            checkout.CartItems = checkoutRequest.CartItems;
            checkout.Recipient = checkoutRequest.Recipient;

            var createPrintFullOrderResult = await CreatePrintfullOrder(checkoutRequest);

            var createPaypallOrderResult = await CreatePaypallOrder();
            if (IsPaypallOrderCreated(createPaypallOrderResult))
            {
                checkout.PaypallOrderId = createPaypallOrderResult.Data.Id;
                // kreiraj order u bazi
            }

            return createPaypallOrderResult;
            //dodaj u header orderId
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

                return Result<CostCalculation>.Success(costCalculation);
            }
            else
            {
                return Result<CostCalculation>.Failure(new Error(result.Error.Message));
            }
            
        }

        private async Task<ApiResponse<Order>> CreatePaypallOrder()
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
                                CurrencyCode = "USD", MValue = "100",
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

        private bool IsPaypallOrderCreated(ApiResponse<Order> order)
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

        public async Task HandleConfirmCheckout()
        {
            
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
                result = await client.PostAsJsonAsync<PrintfullOrderRequest>("/orders", orderBody);

                if (result.IsSuccessStatusCode) 
                {
                    var content = await result.Content.ReadAsStringAsync();
                    return Result<PrintfullOrderResponse>.Success(JsonSerializer.Deserialize<PrintfullOrderResponse>(content));
                }
                
            }
            catch (Exception ex)
            {
            }

            return Result<PrintfullOrderResponse>.Failure(new Error("Message from response"));//error message from printfull api resposnse
        }

        private async Task<PrintfullOrderRequest> CreateOrderBodyForRequest(CheckoutRequest checkoutRequest)
        {
            var orderBody = new PrintfullOrderRequest();
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
