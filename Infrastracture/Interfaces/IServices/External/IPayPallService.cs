
using PaypalServerSdk.Standard.Http.Response;
using PaypalServerSdk.Standard.Models;

namespace Infrastracture.Interfaces.IServices.External
{
    public interface IPayPallService
    {
        Task<ApiResponse<PaypalServerSdk.Standard.Models.Order>> CreatePaypallOrder(string amount);
        Task<ApiResponse<PaypalServerSdk.Standard.Models.Order>> CapturePaypallOrder(string paypallOrderID);
        Task<ApiResponse<Refund>> RefundCapturedPayment(string captureId);
    }
}
