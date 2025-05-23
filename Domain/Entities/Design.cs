using Domain.Enums;
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
        public string? BfImgUrl { get; set; }
        public string? LowResImgUrl { get; set; }
        public string? MockImgUrl { get; set; }
        public string? PrintImgUrl { get; set; }
        public ImageOrientation ImageOrientation { get; set; }
    }
}
