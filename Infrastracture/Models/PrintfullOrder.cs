

using Newtonsoft.Json;

namespace Infrastracture.Models
{
    public class PrintfullOrder
    {
        [JsonProperty("recipient")]
        public PrintfullOrderRecipient Recipient { get; set; }

        [JsonProperty("items")]
        public List<PrintfullOrderItem> Items { get; set; }
    }

    public class PrintfullOrderRecipient
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state_code")]
        public string StateCode { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    
    }

    public class PrintfullOrderItem
    {
        [JsonProperty("variant_id")]
        public long VariantId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("files")]
        public List<FileForPrinting> Files { get; set; }
    }

    public class FileForPrinting
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

}
