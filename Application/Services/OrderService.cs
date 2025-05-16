using Application.Helpers;
using Domain.Entities;
using Infrastracture.Interfaces.IRepositories;
using Infrastracture.Interfaces.IServices;
using Infrastracture.Interfaces.IServices.External;
using Infrastracture.Models;
using Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;
        private readonly IPrintfullService _printfullService;

        public OrderService(IOrderRepository orderRepository, ILogger<PosterService> logger, IPrintfullService printfullService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _printfullService = printfullService;
        }
        
        public async Task<Result<List<OrderModel>>> GetOrdersForUser(string userId)
        {
            try
            {
                var orders = await _orderRepository.GetUserOrders(userId);
                var orderModels = new List<OrderModel>();
                foreach (var order in orders)
                {
                    var orderItems = new List<OrderItemModel>();
                    var designModel = new DesignModel();

                    foreach (var orderItem in order.OrderItems)
                    {
                        designModel = new DesignModel
                        {
                            DesignName = orderItem.Design.DesignName,
                            ImgUrl = orderItem.Design.ImgUrl
                        };
                        var orderItemModel = new OrderItemModel
                        {
                            Quantity = orderItem.Quantity,
                            Design = designModel,
                            ProductId = orderItem.ProductId
                        };

                        orderItems.Add(orderItemModel);
                    }

                    var orderModel = new OrderModel
                    {
                        PrintfullOrderId = order.PrintfullOrderId,
                        TotalCost = order.TotalCost,
                        OrderItems = orderItems,
                    };
                    orderModels.Add(orderModel);
                }

                return Result<List<OrderModel>>.Success(orderModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: DesignService.GetOrdersForRecipient()");
                throw;
            }
        }
    }
}
