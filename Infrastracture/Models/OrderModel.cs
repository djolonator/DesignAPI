using Domain.Entities;

namespace Infrastracture.Models
{
    public class OrderModel
    {
        public long PrintfullOrderId { get; set; }
        public decimal TotalCost { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
    }
}
