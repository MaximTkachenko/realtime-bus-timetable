using System;
using System.Text.Json.Serialization;

namespace Models
{
    public class BusStop
    {
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("x")] public double X { get; set; }
        [JsonPropertyName("y")] public double Y { get; set; }

        public bool Equals(BusStop other)
        {
            return Id.Equals(other.Id);
        }

        public double GetDistance(BusStop anotherBusStop) => GetDistance(anotherBusStop.X, anotherBusStop.Y);

        public double GetDistance(Location location) => GetDistance(location.X, location.Y);

        private double GetDistance(double x, double y) => Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2));
    }
}
