
namespace Infrastracture.Models
{
    public class DesignModel
    {
        public int DesignId { get; set; }
        public string DesignName { get; set; }
        public string Description { get; set; }
        public int DesignCategoryId { get; set; }
        public string? ImgUrl { get; set; }
        public string? MockUrl { get; set; }

    }
}
