
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public int PrintfullOrderId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string ContactEmail { get; set; }
        public string RecipientName { get; set; }
        public string RecipientFullName { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientCountry { get; set; }
        public string RecipientCity { get; set; }
    }
}
