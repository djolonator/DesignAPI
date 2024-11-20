
using Infrastracture.Models;
using Infrastructure.Abstractions;
using PaypalServerSdk.Standard.Http.Response;
using PaypalServerSdk.Standard.Models;

namespace Infrastracture.Interfaces.IServices
{
    public interface ICheckoutService
    {
        Task<Result<ApiResponse<Order>>> HandleInitiatePaypallOrder(string userId);
        Task<Result<CostCalculation>> CalculateTotalCost(CheckoutRequest checkoutRequest, string userId);
    }
}
