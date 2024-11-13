

using Domain.Entities;

namespace Infrastracture.Interfaces.IRepositories
{
    public interface IOrderRepository
    {
        public Task<long> CreateOrder(Order order);
        public Task<Order?> FindOrderById(int id);
    }
}
