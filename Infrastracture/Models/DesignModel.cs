
namespace Infrastracture.Models
{
    public class DesignModel
    {
        public long DesignId { get; set; }
        public string DesignName { get; set; }
        public string Description { get; set; }
        public long DesignCategoryId { get; set; }
        public string? ImgUrl { get; set; }
        public string? MockUrl { get; set; }

    }
}
