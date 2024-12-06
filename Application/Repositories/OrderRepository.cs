

using Domain;
using Domain.Entities;
using Infrastracture.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public void SaveChanges()
        {
            _storageContext.SaveChanges();
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

        public async Task<Order?> FindOrderByUserId(string userId, bool isCurrent = false)
        {
            return await _storageContext.Order
                .Include(o => o.Recipient)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Current == isCurrent);
        }

        public async Task<Order?> FindOrderByUserIdNoTracking(string userId, bool isCurrent = false)
        {
            return await _storageContext.Order.AsNoTracking().FirstOrDefaultAsync(o => o.UserId == userId);
        }

        public async Task DeleteOrder(string userId)
        {
            var order = await FindOrderByUserId(userId);
            if (order != null) 
            {
                _storageContext.Order.Remove(order);
                _storageContext.SaveChanges();
            }
        }

        public EntityEntry<Order> DeleteOrder(Order userOrder)
        {
            var result = _storageContext.Order.Remove(userOrder);

            return result;
        }
    }
}
