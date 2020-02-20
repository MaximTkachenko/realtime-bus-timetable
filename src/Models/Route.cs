using System.Text.Json.Serialization;

namespace Models
{
    public class Route
    {
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("color")] public string Color { get; set; }
        [JsonPropertyName("path")] public Point[] Path { get; set; }

        public class Point
        {
            [JsonPropertyName("x")] public int X { get; set; }
            [JsonPropertyName("y")] public int Y { get; set; }
            [JsonPropertyName("busStopId")] public string BusStopId { get; set; }
            [JsonPropertyName("isBusStop")] public bool IsBusStop => !string.IsNullOrEmpty(BusStopId);
        }
    }
}
