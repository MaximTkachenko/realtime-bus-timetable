using System;
using BusTimetable.Generator.Models;

namespace BusTimetable.Generator.Generators
{
    static class RouteGenerator
    {
        public static Route[] Generate(BusStop[] busStops, int width, int height, int routesNumber)
        {
            var routes = new Route[routesNumber];
            for (var i = 0; i < routesNumber; i++)
            {
                routes[i] = new Route
                {
                    Id = GetId(),
                    Color = GetColor(),
                    Path = GetPath()
                };
            }

            return routes;
        }

        private static string GetColor()
        {
            throw new NotImplementedException();
        }

        private static string GetId()
        {
            throw new NotImplementedException();
        }

        private static Route.Point[] GetPath()
        {
            throw new NotImplementedException();
        }
    }
}
