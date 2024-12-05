using System.Text.Json.Serialization;


namespace Infrastracture.Models
{
    public class EstimatePrintfullOrderCosts
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("result")]
        public Result Result { get; set; }

        [JsonPropertyName("extra")]
        public object[] Extra { get; set; }
    }

    public class Result
    {
        [JsonPropertyName("costs")]
        public CalculateCosts Costs { get; set; }

        [JsonPropertyName("retail_costs")]
        public CalculateRetailCosts RetailCosts { get; set; }
    }

    public class CalculateCosts
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("subtotal")]
        public decimal Subtotal { get; set; }

        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }

        [JsonPropertyName("shipping")]
        public decimal Shipping { get; set; }

        [JsonPropertyName("digitization")]
        public decimal Digitization { get; set; }

        [JsonPropertyName("additional_fee")]
        public decimal AdditionalFee { get; set; }

        [JsonPropertyName("fulfillment_fee")]
        public decimal FulfillmentFee { get; set; }

        [JsonPropertyName("retail_delivery_fee")]
        public decimal RetailDeliveryFee { get; set; }

        [JsonPropertyName("tax")]
        public decimal Tax { get; set; }

        [JsonPropertyName("vat")]
        public decimal Vat { get; set; }

        [JsonPropertyName("total")]
        public decimal Total { get; set; }
    }

    public class CalculateRetailCosts
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("subtotal")]
        public decimal? Subtotal { get; set; }

        [JsonPropertyName("discount")]
        public decimal? Discount { get; set; }

        [JsonPropertyName("shipping")]
        public decimal? Shipping { get; set; }

        [JsonPropertyName("tax")]
        public decimal? Tax { get; set; }

        [JsonPropertyName("vat")]
        public decimal? Vat { get; set; }

        [JsonPropertyName("total")]
        public decimal? Total { get; set; }
    }
}
