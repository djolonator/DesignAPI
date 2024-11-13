

using Infrastracture.Models;
using PaypalServerSdk.Standard.Http.Response;
using PaypalServerSdk.Standard.Models;

namespace Infrastracture.Interfaces.IServices
{
    public interface ICheckoutService
    {
        Task<ApiResponse<Order>> HandleInitiateCheckout(CheckoutRequest checkoutRequest);
        Task HandleConfirmCheckout();
    }
}
