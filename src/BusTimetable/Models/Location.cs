using System.Text.Json.Serialization;

namespace BusTimetable.Models
{
    public class Location
    {
        [JsonPropertyName("x")] public float X { get; set; }
        [JsonPropertyName("y")] public float Y { get; set; }
    }
}
