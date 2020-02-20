using System;
using System.Collections.Generic;
using Models;

namespace BusTimetable.Generator.Generators
{
    static class RouteGenerator
    {
        private static readonly Random Rnd = new Random(Guid.NewGuid().GetHashCode());

        public static Route[] Generate(BusStop[] busStops, int width, int height, int routesNumber)
        {
            var routes = new Route[routesNumber];
            for (var i = 0; i < routesNumber; i++)
            {
                routes[i] = new Route
                {
                    Id = IdGenerator.GetId(i),
                    Color = ColorHexGenerator.GetColor(),
                    Path = GetPath(busStops, width, height)
                };
            }

            return routes;
        }

        private static Route.Point[] GetPath(BusStop[] busStops, int width, int height)
        {
            var list = new List<BusStop>(busStops);
            int numberOfStops = busStops.Length / 2 + (int)(busStops.Length * 0.4 / 2);
            var route = new Route.Point[numberOfStops];
            for (int i = 0; i < numberOfStops; i++)
            {
                var stop = list[Rnd.Next(0, list.Count)];
                list.Remove(stop);
                route[i] = new Route.Point
                {
                    BusStopId = stop.Id,
                    X = stop.X,
                    Y = stop.Y
                };
            }
            
            return route;
        }
    }
}
