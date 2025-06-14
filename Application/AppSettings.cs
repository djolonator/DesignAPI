namespace Application
{
    public record AppSettings
    {
        public string? PayPallClientID { get; set; }
        public string? PayPallClientSecret { get; set; }
        public string? CorsPolicy { get; set; }

    }
}
