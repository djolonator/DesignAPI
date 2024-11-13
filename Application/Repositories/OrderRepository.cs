

using Domain;
using Domain.Entities;
using Infrastracture.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly StorageDbContext _storageContext;
        private readonly ILogger _logger;

        public OrderRepository(StorageDbContext storageDbContext, ILogger<OrderRepository> logger)
        {
            _storageContext = storageDbContext;
            _logger = logger;
        }

        public async Task<long> CreateOrder(Order order)
        {
            await _storageContext.Order.AddAsync(order);
            _storageContext.SaveChanges();
            return order.OrderId;
        }

        public async Task<Order?> FindOrderById(int id)
        {
            return await _storageContext.Order.FirstOrDefaultAsync(o => o.OrderId == id);
        }
    }
}
