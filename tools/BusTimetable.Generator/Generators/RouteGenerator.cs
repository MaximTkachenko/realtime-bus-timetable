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
                    //Path = new int[0]
                };
            }

            return routes;
        }

        private static int GetDistance(int x1, int y1, int x2, int y2)
        {
            return (int)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }
    }
}
