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
        private const int Size = 800;
        private const int BustStopsNumber = 20;
        private const int RoutesNumber = 3;
        private const decimal Velocity = 0.1m; //pixels in milliseconds

        static void Main()
        {
            var busStops = BusStopGenerator.Generate(Size, BustStopsNumber);
            var routes = RouteGenerator.Generate(busStops, Size, Velocity, RoutesNumber);

            var json = JsonSerializer.Serialize(new Root
            {
                Width = Size,
                Height = Size,
                BusStops = busStops, 
                Routes = routes
            }, new JsonSerializerOptions{WriteIndented = true});
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"metadata-{Guid.NewGuid()}.json");
            File.AppendAllText(path, json);
        }
    }
}
