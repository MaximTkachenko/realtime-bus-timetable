using System;
using System.Text.Json.Serialization;

namespace Models
{
    public class BusStop
    {
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("color")] public string Color { get; set; }
        [JsonPropertyName("x")] public float X { get; set; }
        [JsonPropertyName("y")] public float Y { get; set; }

        public int GetDistance(BusStop anotherBusStop)
        {
            return GetDistance(anotherBusStop.X, anotherBusStop.Y);
        }

        public int GetDistance(Location location)
        {
            return GetDistance(location.X, location.Y);
        }

        private int GetDistance(float x, float y)
        {
            return (int)Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2));
        }

        public static BusStop NoStop => new BusStop{Id = "NO_BUS_STOP"};
    }
}
