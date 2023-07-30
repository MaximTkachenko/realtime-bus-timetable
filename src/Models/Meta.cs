using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models;

public class Meta
{
    [JsonPropertyName("buses")]
    public Dictionary<string, Bus> Buses { get; set; }
    
    public class Bus
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("route")]
        public Location[] Route { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("lat")]
        public double Latitude { get; set; }
        
        [JsonPropertyName("lon")]
        public double Longitude { get; set; }
    }
}