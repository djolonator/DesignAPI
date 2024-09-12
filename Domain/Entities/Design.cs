using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Design
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DesignId { get; set; }
        public string DesignName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }

    }
}
