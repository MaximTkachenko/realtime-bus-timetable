using System;
using System.Text.Json.Serialization;

namespace Models
{
    public class BusStop
    {
        public static BusStop NoStop => new BusStop { Id = "NO_BUS_STOP" };

        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("color")] public string Color { get; set; }
        [JsonPropertyName("x")] public double X { get; set; }
        [JsonPropertyName("y")] public double Y { get; set; }

        public double GetDistance(BusStop anotherBusStop) => GetDistance(anotherBusStop.X, anotherBusStop.Y);

        public double GetDistance(Location location) => GetDistance(location.X, location.Y);

        private double GetDistance(double x, double y) => Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2));
    }
}
