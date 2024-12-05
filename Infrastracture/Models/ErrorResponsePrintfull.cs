

using System.Text.Json.Serialization;

namespace Infrastracture.Models
{
    public class ErrorResponsePrintfull
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("result")]
        public string Result { get; set; }

        [JsonPropertyName("error")]
        public ErrorDetail Error { get; set; }
    }

    public class ErrorDetail
    {
        [JsonPropertyName("reason")]
        public string Reason { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
