

namespace Infrastracture.Models
{
    public class OrderDetailModel
    {
        public string Status { get; set; }
        public string Shipping { get; set; }
        public string ShippingServiceName { get; set; }
        public RecipientModel Recipient { get; set; }
        public List<string> TrackingUrls { get; set; } = new();
     
    }
}
