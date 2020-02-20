using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using BusTimetable.Generator.Generators;
using Models;

namespace BusTimetable.Generator
{
    class Program
    {
        private const int Width = 1000;
        private const int Height = 800;
        private const int BustStopsNumber = 20;
        private const int RoutesNumber = 3;

        static void Main()
        {
            var busStops = BusStopGenerator.Generate(Width, Height, BustStopsNumber);
            var routes = RouteGenerator.Generate(busStops, Width, Height, RoutesNumber);

            var json = JsonSerializer.Serialize(new Root
            {
                Width = Width,
                Height = Height,
                BusStops = busStops, 
                Routes = routes
            }, new JsonSerializerOptions{WriteIndented = true});
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"metadata-{Guid.NewGuid()}.json");
            File.AppendAllText(path, json);
        }
    }
}
