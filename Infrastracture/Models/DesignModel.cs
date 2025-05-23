
namespace Infrastracture.Models
{
    public class DesignModel
    {
        public long DesignId { get; set; }
        public string DesignName { get; set; }
        public string Description { get; set; }
        public string? BfImgUrl { get; set; }
        public string? LowResImgUrl { get; set; }
        public string? MockImgUrl { get; set; }
        public string? PrintImgUrl { get; set; }
    }
}
