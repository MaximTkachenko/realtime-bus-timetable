using System.Text.Json.Serialization;

namespace Models
{
    public class Root
    {
        [JsonPropertyName("width")] public int Width { get; set; }
        [JsonPropertyName("height")] public int Height { get; set; }
        [JsonPropertyName("timeSpentOnBusStopMs")] public int TimeSpentOnBusStopMs { get; set; }
        [JsonPropertyName("busStopColor")] public string BusStopColor { get; set; }
        [JsonPropertyName("trackRoutesIntervalMs")] public int TrackRoutesIntervalMs { get; set; }
        [JsonPropertyName("updateIntervalMs")] public int UpdateIntervalMs { get; set; }
        [JsonPropertyName("nowThresholdSec")] public int NowThresholdSec { get; set; }
        [JsonPropertyName("busStops")] public BusStop[] BusStops { get; set; }
        [JsonPropertyName("routes")] public Route[] Routes { get; set; }
    }
}
