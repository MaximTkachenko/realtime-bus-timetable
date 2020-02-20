using System.Text.Json.Serialization;

namespace Models
{
    public class Root
    {
        [JsonPropertyName("width")] public int Width { get; set; }
        [JsonPropertyName("height")] public int Height { get; set; }
        [JsonPropertyName("busStops")] public BusStop[] BusStops { get; set; }
        [JsonPropertyName("routes")] public Route[] Routes { get; set; }
    }
}
