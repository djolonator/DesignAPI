using Infrastracture.Models;
using Infrastructure.Abstractions;

namespace Infrastracture.Interfaces.IServices
{
    public interface IOrderService
    {
        Task<Result<List<OrderModel>>> GetOrdersForUser(string userId);
    }
}
