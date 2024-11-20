using System.Text.Json.Serialization;

namespace Infrastracture.Models
{
    public class PrintfullOrderResponseGet
    {
        [JsonPropertyName("code")]
        public int? Code { get; set; }

        [JsonPropertyName("result")]
        public OrderResultGet Result { get; set; }
    }

    public class OrderResultGet
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; }

        [JsonPropertyName("store")]
        public int? Store { get; set; }

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
        public RecipientGet Recipient { get; set; }

        [JsonPropertyName("items")]
        public List<OrderItemGet> Items { get; set; }

        [JsonPropertyName("branding_items")]
        public List<OrderItemGet> BrandingItems { get; set; }

        [JsonPropertyName("incomplete_items")]
        public List<IncompleteItem> IncompleteItems { get; set; }

        [JsonPropertyName("costs")]
        public CostsGet Costs { get; set; }

        [JsonPropertyName("retail_costs")]
        public CostsGet RetailCosts { get; set; }

        [JsonPropertyName("pricing_breakdown")]
        public List<PricingBreakdown> PricingBreakdown { get; set; }

        [JsonPropertyName("shipments")]
        public List<Shipment> Shipments { get; set; }

        [JsonPropertyName("gift")]
        public Gift Gift { get; set; }

        [JsonPropertyName("packing_slip")]
        public PackingSlip PackingSlip { get; set; }
    }

    public class RecipientGet
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

    public class OrderItemGet
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; }

        [JsonPropertyName("variant_id")]
        public int?  VariantId { get; set; }

        [JsonPropertyName("sync_variant_id")]
        public int? SyncVariantId { get; set; }

        [JsonPropertyName("external_variant_id")]
        public string ExternalVariantId { get; set; }

        [JsonPropertyName("warehouse_product_variant_id")]
        public int? WarehouseProductVariantId { get; set; }

        [JsonPropertyName("product_template_id")]
        public int? ProductTemplateId { get; set; }

        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("retail_price")]
        public string RetailPrice { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("product")]
        public ProductGet Product { get; set; }

        [JsonPropertyName("files")]
        public List<FileGet> Files { get; set; }

        [JsonPropertyName("options")]
        public List<Option> Options { get; set; }

        [JsonPropertyName("sku")]
        public string Sku { get; set; }

        [JsonPropertyName("discontinued")]
        public bool Discontinued { get; set; }

        [JsonPropertyName("out_of_stock")]
        public bool OutOfStock { get; set; }
    }

    public class ProductGet
    {
        [JsonPropertyName("variant_id")]
        public int? VariantId { get; set; }

        [JsonPropertyName("product_id")]
        public int? ProductId { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class FileGet
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("options")]
        public List<Option> Options { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("filename")]
        public string Filename { get; set; }

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

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
    }

    public class Option
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class IncompleteItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }

        [JsonPropertyName("sync_variant_id")]
        public int? SyncVariantId { get; set; }

        [JsonPropertyName("external_variant_id")]
        public string ExternalVariantId { get; set; }

        [JsonPropertyName("external_line_item_id")]
        public string ExternalLineItemId { get; set; }
    }

    public class CostsGet
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

    public class PricingBreakdown
    {
        [JsonPropertyName("customer_pays")]
        public string CustomerPays { get; set; }

        [JsonPropertyName("printful_price")]
        public string PrintfulPrice { get; set; }

        [JsonPropertyName("profit")]
        public string Profit { get; set; }

        [JsonPropertyName("currency_symbol")]
        public string CurrencySymbol { get; set; }
    }

    public class Shipment
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("carrier")]
        public string Carrier { get; set; }

        [JsonPropertyName("service")]
        public string Service { get; set; }

        [JsonPropertyName("tracking_number")]
        public int? TrackingNumber { get; set; }

        [JsonPropertyName("tracking_url")]
        public string TrackingUrl { get; set; }

        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("ship_date")]
        public string ShipDate { get; set; }

        [JsonPropertyName("shipped_at")]
        public long ShippedAt { get; set; }

        [JsonPropertyName("reshipment")]
        public bool Reshipment { get; set; }

        [JsonPropertyName("items")]
        public List<ShipmentItem> Items { get; set; }
    }

    public class ShipmentItem
    {
        [JsonPropertyName("item_id")]
        public int? ItemId { get; set; }

        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }

        [JsonPropertyName("picked")]
        public int? Picked { get; set; }

        [JsonPropertyName("printed")]
        public int? Printed { get; set; }
    }

    public class Gift
    {
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class PackingSlip
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
