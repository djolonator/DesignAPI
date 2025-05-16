using Infrastracture.Interfaces.IServices;
using Infrastracture.Models;
using PaypalServerSdk.Standard.Http.Response;
using Infrastracture.Interfaces.IRepositories;
using Infrastructure.Abstractions.Errors;
using Infrastructure.Abstractions;
using Application.Helpers;
using Domain.Entities;
using Order = PaypalServerSdk.Standard.Models.Order;
using Infrastracture.Interfaces.IServices.External;


namespace Application.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDesignRepository _designRepository;
        private readonly IPayPallService _payPallService;
        private readonly IPrintfullService _printfullService;

        public CheckoutService(IPrintfullService printfullService, IOrderRepository orderRepository, IDesignRepository designRepository, IPayPallService payPallService)
        {
            _orderRepository = orderRepository;
            _payPallService = payPallService;
            _printfullService = printfullService;
            _designRepository = designRepository;
        }

        public async Task<Result<ApiResponse<PaypalServerSdk.Standard.Models.Order>>> HandleInitiatePaypallOrder(string userId)
        {
            var userOrder = await _orderRepository.FindOrderByUserId(userId, true);
            if (userOrder != null)
            {
                var createPaypallOrderResult = await _payPallService.CreatePaypallOrder(userOrder.TotalCost.ToString());
                if (IsPaypallOrderRequestSuccess(createPaypallOrderResult))
                {
                    userOrder.PaypallOrderId = createPaypallOrderResult.Data.Id;
                    _orderRepository.SaveChanges();
                    return Result<ApiResponse<Order>>.Success(createPaypallOrderResult);
                }
                else
                {
                    //see if delete here
                    var deleteResult = _orderRepository.DeleteOrder(userOrder);
                    _orderRepository.SaveChanges();
                }
            }

            return Result<ApiResponse<Order>>.Failure(new Error("Something went wrong with order processing"));
        }

        public async Task<Result<ApiResponse<PaypalServerSdk.Standard.Models.Order>>> HandleCapturePaypallOrder(string paypallOrderId, string userId)
        {
            var capturePaypallRequestResult = await _payPallService.CapturePaypallOrder(paypallOrderId);
            if (capturePaypallRequestResult.StatusCode == 201)
            {
                var userOrder = await _orderRepository.FindOrderByUserId(userId, true);

                if (userOrder != null)
                {
                    var orderBody = await CreateOrderBodyForRequest(userOrder);
                    var createPrintfullOrderResult = await _printfullService.CreatePrintfullOrder(userOrder, orderBody);

                    if (createPrintfullOrderResult.IsSuccess)
                    {
                        userOrder.PrintfullOrderId = createPrintfullOrderResult.Value!.Result.Id;
                        userOrder.PaypallCaptureId = capturePaypallRequestResult.Data.PurchaseUnits[0].Payments.Captures[0].Id;
                        long printfullOrderId = userOrder.PrintfullOrderId;
                        userOrder.Current = false;
                        _orderRepository.SaveChanges();
                        
                    }
                }
            }

            //remove paypall order?

            return Result<ApiResponse<PaypalServerSdk.Standard.Models.Order>>.Success(capturePaypallRequestResult);
        }

        public async Task<Result<CostCalculation>> EstimateTotalCost(CheckoutRequest checkoutRequest, string userId)
        {
            var checkout = new CheckoutRequest();
            checkout.CartItems = checkoutRequest.CartItems;
            checkout.Recipient = checkoutRequest.Recipient;

            var userOrderModel = MapCheckoutRequestToOrder(checkoutRequest);
            var orderBody = await CreateOrderBodyForRequest(userOrderModel);

            var result = await _printfullService.EstimatePrintfullOrderCosts(checkoutRequest, orderBody);

            if (result.IsSuccess)
            {
                var costCalculation = new CostCalculation();
                costCalculation.ItemsCost = PriceCalculator.AddKaymakToItemPrice(result.Value!.Result.Costs.Subtotal);
                costCalculation.ShippingCost = PriceCalculator.AddKaymakToShiping(result.Value.Result.Costs.Shipping);
                costCalculation.TotalCost = costCalculation.ItemsCost + costCalculation.ShippingCost;
                var orderItems = new List<OrderItem>();

                checkout.CartItems.ForEach(cartItem =>
                {
                    var orderItem = new OrderItem();
                    orderItem.ProductId = cartItem.ProductId;
                    orderItem.Quantity = cartItem.Quantity;
                    orderItem.DesignId = cartItem.DesignId;

                    orderItems.Add(orderItem);
                });

                var userOrder = await _orderRepository.FindOrderByUserId(userId, true);

                if (userOrder == null)
                {
                    var orderId = await _orderRepository.CreateOrder(new Domain.Entities.Order()
                    {
                        UserId = userId,
                        OrderItems = orderItems,
                        Current = true,
                        TotalCost = costCalculation.TotalCost,
                        Recipient = new Recipient
                        {
                            Address = checkoutRequest.Recipient!.Address!,
                            Email = checkoutRequest.Recipient.Email!,
                            Phone = checkoutRequest.Recipient.Phone!,
                            FirstName = checkoutRequest.Recipient.FirstName!,
                            LastName = checkoutRequest.Recipient.LastName!,
                            City = checkoutRequest.Recipient.City!,
                            Country = checkoutRequest.Recipient.Country!,
                            Zip = checkoutRequest.Recipient.Zip!
                        }
                    });
                }
                else
                {
                    userOrder.OrderItems = orderItems;
                    userOrder.TotalCost = costCalculation.TotalCost;
                    userOrder.Recipient.Address = checkoutRequest.Recipient!.Address!;
                    userOrder.Recipient.Email = checkoutRequest.Recipient.Email!;
                    userOrder.Recipient.Phone = checkoutRequest.Recipient.Phone!;
                    userOrder.Recipient.FirstName = checkoutRequest.Recipient.FirstName!;
                    userOrder.Recipient.LastName = checkoutRequest.Recipient.LastName!;
                    userOrder.Recipient.City = checkoutRequest.Recipient.City!;
                    userOrder.Recipient.Country = checkoutRequest.Recipient.Country!;
                    userOrder.Recipient.Zip = checkoutRequest.Recipient.Zip!;
                    _orderRepository.SaveChanges();
                }

                return Result<CostCalculation>.Success(costCalculation);
            }

            return Result<CostCalculation>.Failure(new Error("Order costs could not be estimated at this moment"));
        }

        private bool IsPaypallOrderRequestSuccess(ApiResponse<PaypalServerSdk.Standard.Models.Order> order)
        {
            if (order.StatusCode is 201)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<CreatePrintfullOrderRequest> CreateOrderBodyForRequest(Domain.Entities.Order userOrder)
        {
            var orderBody = new CreatePrintfullOrderRequest();
            var recipient = new PrintfullOrderRecipient()
            {
                Name = userOrder.Recipient!.FirstName + " " + userOrder.Recipient.LastName,
                Address1 = userOrder.Recipient!.Address!,
                City = userOrder.Recipient!.City!,
                CountryCode = userOrder.Recipient!.Country!,
                CountryName = Constants.CountriesNames.CountryNames()[userOrder.Recipient.Country],
                Zip = userOrder.Recipient!.Zip!,
                Phone = userOrder.Recipient!.Phone!,
                Email = userOrder.Recipient!.Email!
            };

            var items = new List<PrintfullOrderItem>();

            foreach (var i in userOrder.OrderItems)
            {
                var design = await _designRepository.GetDesignByIdAsync(i.DesignId);
                if (design != null && !string.IsNullOrEmpty(design.ImgForPrintUrl))
                {
                    var item = new PrintfullOrderItem()
                    {
                        Quantity = i.Quantity,
                        VariantId = Constants.Product.ProductVariants()[i.ProductId],
                        Files = new List<FileForPrinting>()
                    };

                    item.Files.Add(new FileForPrinting()
                    {
                        Url = design.ImgForPrintUrl!
                    });

                    items.Add(item);
                }
            }

            orderBody.Recipient = recipient;
            orderBody.Items = items;
            return orderBody;
        }

        private Domain.Entities.Order MapCheckoutRequestToOrder(CheckoutRequest checkout)
        {
            var order = new Domain.Entities.Order();
            order.OrderItems = new List<OrderItem>();
            checkout.CartItems.ForEach(cartItem =>
            {
                var orderItem = new OrderItem();
                orderItem.ProductId = cartItem.ProductId;
                orderItem.Quantity = cartItem.Quantity;
                orderItem.DesignId = cartItem.DesignId;

                order.OrderItems.Add(orderItem);
            });

            order.Recipient = new Recipient
            {
                Address = checkout.Recipient!.Address!,
                Email = checkout.Recipient.Email!,
                Phone = checkout.Recipient.Phone!,
                FirstName = checkout.Recipient.FirstName!,
                LastName = checkout.Recipient.LastName!,
                City = checkout.Recipient.City!,
                Country = checkout.Recipient.Country!,
                Zip = checkout.Recipient.Zip!
            };

            return order;
        }
    }
}
