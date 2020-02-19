using System;
using BusTimetable.Generator.Models;

namespace BusTimetable.Generator.Generators
{
    static class BusStopGenerator
    {
        public static BusStop[] Generate(int width, int height, int busStopsNumber)
        {
            var xRandom = new Random(Guid.NewGuid().GetHashCode());
            var yRandom = new Random(Guid.NewGuid().GetHashCode());

            var stops = new BusStop[busStopsNumber];
            for (var i = 0; i < busStopsNumber; i++)
            {
                stops[i] = new BusStop
                {
                    Id = GetId(),
                    Color = GetColor(),
                    X = xRandom.Next(0, width),
                    Y = yRandom.Next(0, height)
                };
            }

            return stops;
        }

        private static string GetColor()
        {
            throw new NotImplementedException();
        }

        private static string GetId()
        {
            throw new NotImplementedException();
        }
    }
}
