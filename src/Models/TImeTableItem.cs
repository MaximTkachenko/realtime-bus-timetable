using System.Text.Json.Serialization;

namespace Models
{
    public class TimeTableItem
    {
        [JsonPropertyName("routeId")] public string RouteId { get; set; }
        [JsonPropertyName("msBeforeArrival")] public float MsBeforeArrival { get; set; }
    }
}
