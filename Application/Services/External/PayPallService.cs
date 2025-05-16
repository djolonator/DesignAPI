using Infrastracture.Interfaces.IServices.External;
using Microsoft.Extensions.Options;
using PaypalServerSdk.Standard.Authentication;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Controllers;
using PaypalServerSdk.Standard.Models;
using Microsoft.Extensions.Logging;
using PaypalServerSdk.Standard.Http.Response;

namespace Application.Services.External
{
    public class PayPallService : IPayPallService
    {
        private readonly OrdersController _ordersController;
        private readonly PaymentsController _paymentsController;
        private readonly Dictionary<string, CheckoutPaymentIntent> _paymentIntentMap;
        private string _paypalClientId;
        private string _paypalClientSecret;

        public PayPallService(IOptions<AppSettings> appSettings)
        {
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

        public async Task<ApiResponse<PaypalServerSdk.Standard.Models.Order>> CreatePaypallOrder(string amount)
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

            return new ApiResponse<PaypalServerSdk.Standard.Models.Order>(500, new Dictionary<string, string>(), new PaypalServerSdk.Standard.Models.Order());
        }

        public async Task<ApiResponse<PaypalServerSdk.Standard.Models.Order>> CapturePaypallOrder(string paypallOrderID)
        {
            OrdersCaptureInput ordersCaptureInput = new OrdersCaptureInput
            {
                Id = paypallOrderID,
            };

            ApiResponse<PaypalServerSdk.Standard.Models.Order> result = await _ordersController.OrdersCaptureAsync(ordersCaptureInput);

            return result;

        }

        public async Task<ApiResponse<Refund>> RefundCapturedPayment(string captureId)
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
    }
}
