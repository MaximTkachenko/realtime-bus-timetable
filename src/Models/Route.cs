using System.Text.Json.Serialization;

namespace Models
{
    public class Route
    {
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("color")] public string Color { get; set; }
        [JsonPropertyName("path")] public int[] Path { get; set; }
    }
}
