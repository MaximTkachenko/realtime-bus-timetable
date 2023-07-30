using System;
using System.Text.Json.Serialization;
using Orleans;

namespace Models;

[GenerateSerializer]
public class GeoLocation
{
    [Id(0), JsonPropertyName("lat")] public double Latitude { get; set; }
    [Id(1), JsonPropertyName("lon")] public double Longitude { get; set; }
    [Id(2), JsonPropertyName("ts")] public DateTime Timestamp { get; set; }
}