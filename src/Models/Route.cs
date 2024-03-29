﻿using System.Text.Json.Serialization;

namespace Models;

public class Route
{
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("color")] public string Color { get; set; }
    [JsonPropertyName("velocity")] public double Velocity { get; set; }
    [JsonPropertyName("path")] public Point[] Path { get; set; }
}

public class Point
{
    [JsonPropertyName("busStopIndex")] public int BusStopIndex { get; set; }
    [JsonPropertyName("duration")] public double Duration { get; set; }
}