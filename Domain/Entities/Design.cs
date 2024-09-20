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

        [ForeignKey("DesignCategory")]
        public int DesignCategoryId { get; set; }
        public string? ImgUrl { get; set; }
        public string? MockUrl { get; set; }

    }
}
