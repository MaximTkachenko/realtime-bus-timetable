using System.Text.Json.Serialization;
using Orleans;

namespace Models.Timetable
{
    [GenerateSerializer]
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

        [Id(0), JsonPropertyName("routeId")] public string RouteId { get; set; }
        [Id(1), JsonPropertyName("msBeforeArrival")] public double MsBeforeArrival { get; set; }
        [Id(2), JsonPropertyName("direction")] public Direction Direction { get; set; }
        [Id(3), JsonPropertyName("unixTimestamp")] public double UnixTimestamp { get; set; }
    }
}
