using System.Text.Json.Serialization;

namespace IpLookup.Infrastructure.Api.Models
{
    internal class TimeZone
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("current_time")]
        public string CurrentTime { get; set; }

        [JsonPropertyName("current_time_unix")]
        public double CurrentTimeUnix { get; set; }

        [JsonPropertyName("is_dst")]
        public bool IsDst { get; set; }

        [JsonPropertyName("dst_savings")]
        public int DstSavings { get; set; }
    }
}
