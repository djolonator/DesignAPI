namespace Application
{
    public record AppSettings
    {
        public string? PAYPAL_CLIENT_ID { get; set; }
        public string? PAYPAL_CLIENT_SECRET { get; set; }
        public string? CORS_POLICY { get; set; }

    }
}
