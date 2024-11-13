

using Infrastracture.Models;

namespace Infrastracture.Interfaces.IServices
{
    public interface ICheckoutService
    {
        Task HandleCheckout(CheckoutRequest checkout);
    }
}
