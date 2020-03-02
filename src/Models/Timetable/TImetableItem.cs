using System.Text.Json.Serialization;

namespace Models.Timetable
{
    public class TimeTableItem
    {
        [JsonPropertyName("routeId")] public string RouteId { get; set; }
        [JsonPropertyName("msBeforeArrival")] public double MsBeforeArrival { get; set; }
    }
}
