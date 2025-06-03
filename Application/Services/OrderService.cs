
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
        private readonly IPayPallService _payPallService;

        public OrderService(IOrderRepository orderRepository, ILogger<PosterService> logger, IPrintfullService printfullService, IPayPallService payPallService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _printfullService = printfullService;
            _payPallService = payPallService;
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
                            LowResImgUrl = orderItem.Design.LowResImgUrl
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
                        OrderId = order.OrderId,
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

        public async Task<Result<OrderDetailModel>> GetOrderDetails(long orderId)
        {
            try
            {
                var order = await _orderRepository.FindOrderById(orderId);

                if (order is not null)
                {
                    var printfullOrderDetailsReponse = await _printfullService.GetPrintfullOrder(order.PrintfullOrderId);

                    if (printfullOrderDetailsReponse.IsSuccess)
                    {
                        var trackingUrls = new List<string>();

                        foreach(var s in printfullOrderDetailsReponse.Value!.Result.Shipments)
                        {
                            trackingUrls.Add(s.TrackingUrl);
                        }

                        var orderDetailModel = new OrderDetailModel()
                        {
                            Status = printfullOrderDetailsReponse.Value!.Result.Status,
                            Shipping = printfullOrderDetailsReponse.Value.Result.Shipping,
                            ShippingServiceName = printfullOrderDetailsReponse.Value.Result.ShippingServiceName,
                            TrackingUrls = trackingUrls,
                            Recipient = new RecipientModel()
                            {
                                FirstName = printfullOrderDetailsReponse.Value.Result.Recipient.Name,
                                Address = printfullOrderDetailsReponse.Value.Result.Recipient.Address1,
                                Email = printfullOrderDetailsReponse.Value.Result.Recipient.Email
                            }
                        };

                        return Result<OrderDetailModel>.Success(orderDetailModel);
                    }
                    
                }

                return Result<OrderDetailModel>.Failure(new Infrastructure.Abstractions.Errors.Error("Could not get Order Details"));
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: DesignService.GetOrderDetails()");
                throw;
            }
        }

        public async Task<Infrastructure.Abstractions.Result> OrderCancel(WebhookPayload webhookPayload)
        {
            try
            {
                var printfullOrderId = webhookPayload.Data!.Order!.Id!;
                var order = await _orderRepository.FindOrderByPrintfullId((long)printfullOrderId);

                if (order is not null)
                {
                    var result = await _payPallService.RefundCapturedPayment(order.PaypallCaptureId);

                    if ((result.StatusCode == 200 || result.StatusCode == 201) && result.Data.Status == PaypalServerSdk.Standard.Models.RefundStatus.Completed)
                    {
                        return Infrastructure.Abstractions.Result.Success();
                    }
                }

                _logger.LogError("Error in: OrderService.OrderCancel() with exception {@printfullOrderId}", printfullOrderId);
                return Infrastructure.Abstractions.Result.Failure(new Infrastructure.Abstractions.Errors.Error("Could not refund paypallPayment"));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in: OrderService.OrderCancel() with exception {@ex}", ex);
                throw;
            }
        }
    }
}
