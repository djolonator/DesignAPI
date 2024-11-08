
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public int PrintfullId { get; set; }
        public string ProductName { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
