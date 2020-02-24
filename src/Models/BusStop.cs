using System;
using System.Text.Json.Serialization;

namespace Models
{
    public class BusStop
    {
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("color")] public string Color { get; set; }
        [JsonPropertyName("x")] public int X { get; set; }
        [JsonPropertyName("y")] public int Y { get; set; }

        public int GetDistance(BusStop anotherBusStop)
        {
            return (int)Math.Sqrt(Math.Pow(X - anotherBusStop.X, 2) + Math.Pow(Y - anotherBusStop.Y, 2));
        }
    }
}
