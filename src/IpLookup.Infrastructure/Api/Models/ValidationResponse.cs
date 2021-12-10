using System.Text.Json.Serialization;

namespace IpLookup.Infrastructure.Api.Models
{
    internal class ValidationResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
