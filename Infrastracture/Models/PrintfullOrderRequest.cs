
using System.Text.Json.Serialization;
namespace Infrastracture.Models
{
    public class PrintfullOrderRequest
    {
        [JsonPropertyName("recipient")]
        public PrintfullOrderRecipient Recipient { get; set; }

        [JsonPropertyName("items")]
        public List<PrintfullOrderItem> Items { get; set; }
    }

    public class PrintfullOrderRecipient
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address1")]
        public string Address1 { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state_code")]
        public string StateCode { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }

        [JsonPropertyName("zip")]
        public string Zip { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    
    }

    public class PrintfullOrderItem
    {
        [JsonPropertyName("variant_id")]
        public long VariantId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("files")]
        public List<FileForPrinting> Files { get; set; }
    }

    public class FileForPrinting
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

}
