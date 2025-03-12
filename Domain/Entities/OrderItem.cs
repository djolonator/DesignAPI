
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderItemId { get; set; }
        [ForeignKey("Order")]
        public long OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey("Design")]
        public long DesignId { get; set; }
        public Design Design { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
