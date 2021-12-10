using System.Text.Json.Serialization;

namespace IpLookup.Infrastructure.Api.Models
{
    internal class Currency
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
    }
}
