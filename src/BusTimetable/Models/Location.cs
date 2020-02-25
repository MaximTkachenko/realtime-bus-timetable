using System;
using System.Text.Json.Serialization;
using Models;

namespace BusTimetable.Models
{
    public class Location
    {
        [JsonPropertyName("x")] public float X { get; set; }
        [JsonPropertyName("y")] public float Y { get; set; }

        private const float fuziness = 0.3F;
        //https://stackoverflow.com/questions/907390/how-can-i-tell-if-a-point-belongs-to-a-certain-line
        public bool IsBetween(BusStop stop1, BusStop stop2)
        {
            //todo check in square limited by points
            
            if (Math.Abs(X - stop1.X) < fuziness && Math.Abs(Y - stop1.Y) < fuziness)
            {
                return true;
            }

            if (Math.Abs(X - stop2.X) < fuziness && Math.Abs(Y - stop2.Y) < fuziness)
            {
                return true;
            }

            var deltaX = Math.Abs(stop1.X - stop2.X);
            var deltaY = Math.Abs(stop1.Y - stop2.Y);

            if (Math.Abs(deltaY) < fuziness)
            {
                return Math.Abs(Y - stop1.Y) < fuziness && X <= Math.Max(stop1.X, stop2.X) && X >= Math.Min(stop1.X, stop1.X);
            }

            if (Math.Abs(deltaX) < fuziness)
            {
                return Math.Abs(X - stop1.X) < fuziness && Y <= Math.Max(stop1.Y, stop2.Y) && Y >= Math.Min(stop1.Y, stop2.Y);
            }

            var main = deltaX / deltaY;
            var nomain = (X - Math.Min(stop1.X, stop2.X)) / (Y - Math.Min(stop1.Y, stop2.Y));

            return Math.Abs(main - nomain) < fuziness;
        }
    }
}
