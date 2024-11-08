
namespace Infrastracture.Models
{
    public class OrderModelClientRequest
    {
        public string ContactEmail { get; set; }
        public string RecipientName { get; set; }
        public string RecipientFullName { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientCountry { get; set; }
        public string RecipientCity { get; set; }
    }
}
