using System;
using System.Collections.Generic;
using Models;

namespace BusTimetable.Generator.Generators
{
    static class RouteGenerator
    {
        private static readonly Random Rnd = new Random(Guid.NewGuid().GetHashCode());

        public static Route[] Generate(BusStop[] busStops, int size, decimal velocity, int routesNumber)
        {
            var routes = new Route[routesNumber];
            for (var i = 0; i < routesNumber; i++)
            {
                routes[i] = new Route
                {
                    Id = IdGenerator.GetId(i),
                    Color = ColorHexGenerator.GetColor(),
                    Path = GetPath(busStops, size, velocity)
                };
            }

            return routes;
        }

        private static Route.Point[] GetPath(BusStop[] busStops, int size, decimal velocity)
        {
            var list = new List<BusStop>(busStops);
            int numberOfStops = busStops.Length / 2;
            var route = new Route.Point[numberOfStops];
            for (int i = 0; i < numberOfStops; i++)
            {
                var stop = GetNextBusStop(list, i == 0 ? null : route[i - 1]);
                route[i] = new Route.Point
                {
                    BusStopId = stop.Id,
                    X = stop.X,
                    Y = stop.Y,
                    Duration = i > 0 
                        ? (int)(GetDistance(route[i - 1].X, route[i - 1].Y, stop.X, stop.Y) / velocity)
                        : 0
                };
            }
            
            return route;
        }

        private static BusStop GetNextBusStop(List<BusStop> stops, Route.Point last)
        {
            if (last == null)
            {
                var randomStop = stops[Rnd.Next(0, stops.Count)];
                stops.Remove(randomStop);
                return randomStop;
            }

            var distances = new (int Distance, BusStop stop)[stops.Count];
            for (int i = 0; i < stops.Count; i++)
            {
                distances[i] = (GetDistance(stops[i].X, stops[i].Y, last.X, last.Y), stops[i]);
            }

            Array.Sort(distances, (a, b) => a.Distance.CompareTo(b.Distance));
            var closestStop = distances[Rnd.Next(0, distances.Length - 1)].stop;
            stops.Remove(closestStop);
            return closestStop;
        }

        private static int GetDistance(int x1, int y1, int x2, int y2)
        {
            return (int)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }
    }
}
