
using Infrastracture.Interfaces.IServices;
using Microsoft.Extensions.Options;
using PaypalServerSdk.Standard.Authentication;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Controllers;
using PaypalServerSdk.Standard.Models;
using Microsoft.Extensions.Logging;
using Infrastracture.Models;
using PaypalServerSdk.Standard.Http.Response;
using System.Net.Http.Json;


namespace Application.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OrdersController _ordersController;
        private readonly PaymentsController _paymentsController;
        private readonly Dictionary<string, CheckoutPaymentIntent> _paymentIntentMap;
        private string _paypalClientId;
        private string _paypalClientSecret;
        private CheckoutRequest CHECKOUT;


        public CheckoutService(IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory)
        {
            CHECKOUT = new CheckoutRequest();
            _httpClientFactory = httpClientFactory;
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
        }

        public async Task HandleCheckout(CheckoutRequest checkoutRequest)
        {

            CHECKOUT.CartItems = checkoutRequest.CartItems;
            CHECKOUT.Recipient = checkoutRequest.Recipient;
            
            var createOrderResult = await CreatePaypallOrder();
            if (IsPaypallOrderCreated(createOrderResult))
            {
                CHECKOUT.PaypallOrderId = createOrderResult.Data.Id;
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
        private async Task<dynamic> CaptureOrder(string orderID)
        {
            OrdersCaptureInput ordersCaptureInput = new OrdersCaptureInput
            {
                Id = orderID,
            };

            ApiResponse<Order> result = await _ordersController.OrdersCaptureAsync(ordersCaptureInput);

            return result;
        }


        private async Task CreatePrintfullOrder()
        {
            var client = _httpClientFactory.CreateClient("printfull");
            //client.PostAsJsonAsync("/orders");
            string result = await client.GetStringAsync("/");
        }
    }
}
