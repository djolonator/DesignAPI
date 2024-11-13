
using Newtonsoft.Json;

namespace Infrastracture.Models
{
    public class CheckoutRequest
    {
       public Recipient? Recipient { get; set; }
       public List<CartItem> CartItems { get; set; } = new List<CartItem>();
       public string? PaypallOrderId { get; set; }
       public int PrintfullOrderId { get; set; }
    }

    public record Recipient
    {
        [JsonProperty("phone")]
        public string? Phone { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("firstname")]
        public string? FirstName { get; set; }

        [JsonProperty("lastname")]
        public string? LastName { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("zip")]
        public string? Zip { get; set; }

    }

    public class CartItem
    {
        public int DesignId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
