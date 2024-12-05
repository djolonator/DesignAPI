
using System.Text.Json.Serialization;

namespace Infrastracture.Models
{
    public class PrintfullOrderResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("result")]
        public OrderResult Result { get; set; }

        [JsonPropertyName("extra")]
        public List<object> Extra { get; set; }
    }

    public class OrderResult
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; }

        [JsonPropertyName("store")]
        public long Store { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("shipping")]
        public string Shipping { get; set; }

        [JsonPropertyName("shipping_service_name")]
        public string ShippingServiceName { get; set; }

        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("updated")]
        public long Updated { get; set; }

        [JsonPropertyName("recipient")]
        public RecipientResponse Recipient { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("incomplete_items")]
        public List<object> IncompleteItems { get; set; }

        [JsonPropertyName("is_sample")]
        public bool IsSample { get; set; }

        [JsonPropertyName("needs_approval")]
        public bool NeedsApproval { get; set; }

        [JsonPropertyName("not_synced")]
        public bool NotSynced { get; set; }

        [JsonPropertyName("has_discontinued_items")]
        public bool HasDiscontinuedItems { get; set; }

        [JsonPropertyName("can_change_hold")]
        public bool CanChangeHold { get; set; }

        [JsonPropertyName("costs")]
        public Costs Costs { get; set; }

        [JsonPropertyName("dashboard_url")]
        public string DashboardUrl { get; set; }

        [JsonPropertyName("items")]
        public List<OrderItemResponse> Items { get; set; }

        [JsonPropertyName("branding_items")]
        public List<object> BrandingItems { get; set; }

        [JsonPropertyName("shipments")]
        public List<object> Shipments { get; set; }

        [JsonPropertyName("packing_slip")]
        public string PackingSlip { get; set; }

        [JsonPropertyName("retail_costs")]
        public RetailCosts RetailCosts { get; set; }

        [JsonPropertyName("gift")]
        public object Gift { get; set; }
    }

    public class RecipientResponse
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

    public class Costs
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("subtotal")]
        public double Subtotal { get; set; }

        [JsonPropertyName("discount")]
        public double Discount { get; set; }

        [JsonPropertyName("shipping")]
        public double Shipping { get; set; }

        [JsonPropertyName("digitization")]
        public double Digitization { get; set; }

        [JsonPropertyName("additional_fee")]
        public double AdditionalFee { get; set; }

        [JsonPropertyName("fulfillment_fee")]
        public double FulfillmentFee { get; set; }

        [JsonPropertyName("retail_delivery_fee")]
        public string RetailDeliveryFee { get; set; }

        [JsonPropertyName("tax")]
        public string Tax { get; set; }

        [JsonPropertyName("vat")]
        public int Vat { get; set; }

        [JsonPropertyName("total")]
        public double Total { get; set; }
    }

    public class OrderItemResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; }

        [JsonPropertyName("variant_id")]
        public int VariantId { get; set; }

        [JsonPropertyName("sync_variant_id")]
        public string SyncVariantId { get; set; }

        [JsonPropertyName("external_variant_id")]
        public string ExternalVariantId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("retail_price")]
        public double? RetailPrice { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("product")]
        public Product Product { get; set; }

        [JsonPropertyName("files")]
        public List<File> Files { get; set; }

        [JsonPropertyName("options")]
        public List<object> Options { get; set; }

        [JsonPropertyName("sku")]
        public string Sku { get; set; }

        [JsonPropertyName("discontinued")]
        public bool Discontinued { get; set; }

        [JsonPropertyName("out_of_stock")]
        public bool OutOfStock { get; set; }
    }

    public class Product
    {
        [JsonPropertyName("variant_id")]
        public int VariantId { get; set; }

        [JsonPropertyName("product_id")]
        public int ProductId { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class File
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("filename")]
        public string Filename { get; set; }

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [JsonPropertyName("dpi")]
        public int? Dpi { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        [JsonPropertyName("preview_url")]
        public string PreviewUrl { get; set; }

        [JsonPropertyName("visible")]
        public bool Visible { get; set; }

        [JsonPropertyName("is_temporary")]
        public bool IsTemporary { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class RetailCosts
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("subtotal")]
        public double? Subtotal { get; set; }

        [JsonPropertyName("discount")]
        public double? Discount { get; set; }

        [JsonPropertyName("shipping")]
        public double? Shipping { get; set; }

        [JsonPropertyName("tax")]
        public double? Tax { get; set; }

        [JsonPropertyName("vat")]
        public double? Vat { get; set; }

        [JsonPropertyName("total")]
        public double? Total { get; set; }
    }
}
