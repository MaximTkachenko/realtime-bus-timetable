using System;

namespace BusTimetable.Generator.Generators
{
    public static class ColorHexGenerator
    {
        private static readonly Random Rnd = new Random(Guid.NewGuid().GetHashCode());

        public static string GetColor() => $"#{Rnd.Next(0x1000000):X6}";
    }
}
