using System.Text.Json.Serialization;

namespace Infrastracture.Models
{
    public class WebhookPayload
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("retries")]
        public int Retries { get; set; }

        [JsonPropertyName("store")]
        public int Store { get; set; }

        [JsonPropertyName("data")]
        public WebhookData Data { get; set; }
    }

    public class WebhookData
    {
        [JsonPropertyName("reason")]
        public string Reason { get; set; }

        [JsonPropertyName("order")]
        public WebhookOrder Order { get; set; }
    }

    public class WebhookOrder
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; }

        [JsonPropertyName("store")]
        public int Store { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("shipping")]
        public string Shipping { get; set; }

        [JsonPropertyName("shipping_service_name")]
        public string ShippingServiceName { get; set; }

        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("updated")]
        public long Updated { get; set; }

        [JsonPropertyName("recipient")]
        public WebhookRecipient Recipient { get; set; }

        [JsonPropertyName("items")]
        public List<object> Items { get; set; }

        [JsonPropertyName("branding_items")]
        public List<object> BrandingItems { get; set; }

        [JsonPropertyName("incomplete_items")]
        public List<object> IncompleteItems { get; set; }

        [JsonPropertyName("costs")]
        public WebhookCosts Costs { get; set; }

        [JsonPropertyName("retail_costs")]
        public WebhookCosts RetailCosts { get; set; }

        [JsonPropertyName("pricing_breakdown")]
        public List<object> PricingBreakdown { get; set; }

        [JsonPropertyName("shipments")]
        public List<object> Shipments { get; set; }

        [JsonPropertyName("gift")]
        public WebhookGift Gift { get; set; }

        [JsonPropertyName("packing_slip")]
        public WebhookPackingSlip PackingSlip { get; set; }
    }

    public class WebhookRecipient
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("company")]
        public string Company { get; set; }

        [JsonPropertyName("address1")]
        public string Address1 { get; set; }

        [JsonPropertyName("address2")]
        public string Address2 { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state_code")]
        public string StateCode { get; set; }

        [JsonPropertyName("state_name")]
        public string StateName { get; set; }

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

        [JsonPropertyName("tax_number")]
        public string TaxNumber { get; set; }
    }

    public class WebhookCosts
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("subtotal")]
        public string Subtotal { get; set; }

        [JsonPropertyName("discount")]
        public string Discount { get; set; }

        [JsonPropertyName("shipping")]
        public string Shipping { get; set; }

        [JsonPropertyName("digitization")]
        public string Digitization { get; set; }

        [JsonPropertyName("additional_fee")]
        public string AdditionalFee { get; set; }

        [JsonPropertyName("fulfillment_fee")]
        public string FulfillmentFee { get; set; }

        [JsonPropertyName("retail_delivery_fee")]
        public string RetailDeliveryFee { get; set; }

        [JsonPropertyName("tax")]
        public string Tax { get; set; }

        [JsonPropertyName("vat")]
        public string Vat { get; set; }

        [JsonPropertyName("total")]
        public string Total { get; set; }
    }

    public class WebhookGift
    {
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class WebhookPackingSlip
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("logo_url")]
        public string LogoUrl { get; set; }

        [JsonPropertyName("store_name")]
        public string StoreName { get; set; }

        [JsonPropertyName("custom_order_id")]
        public string CustomOrderId { get; set; }
    }
}
