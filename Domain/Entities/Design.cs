using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Design
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DesignId { get; set; }
        public string DesignName { get; set; }
        public string Description { get; set; }

        [ForeignKey("DesignCategory")]
        public long DesignCategoryId { get; set; }
        public DesignCategory DesignCategory { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string? ImgUrl { get; set; }
        public string? MockUrl { get; set; }
        public string? ImgForPrintUrl { get; set; }

    }
}
