using System.Text.Json.Serialization;

namespace Models.Timetable
{
    public class TimetableItem
    {
        public TimetableItem(){}

        public TimetableItem(string routeId, double msBeforeArrival, Direction direction, double unixTimestamp)
        {
            RouteId = routeId;
            MsBeforeArrival = msBeforeArrival;
            Direction = direction;
            UnixTimestamp = unixTimestamp;
        }

        [JsonPropertyName("routeId")] public string RouteId { get; set; }
        [JsonPropertyName("msBeforeArrival")] public double MsBeforeArrival { get; set; }
        [JsonPropertyName("direction")] public Direction Direction { get; set; }
        [JsonPropertyName("unixTimestamp")] public double UnixTimestamp { get; set; }
    }
}
