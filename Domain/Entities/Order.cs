
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderId { get; set; }
        public string UserId { get; set; }
        public string? PaypallOrderId { get; set; }
        public long PrintfullOrderId { get; set; }
        
    }
}
