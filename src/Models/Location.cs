using System;
using System.Text.Json.Serialization;

namespace Models
{
    public class Location
    {
        private const double SelectionFuzziness = 3; 
        private const double Tolerance = 0.1f;

        [JsonPropertyName("x")] public float X { get; set; }
        [JsonPropertyName("y")] public float Y { get; set; }

        /// <summary>
        /// Based on https://stackoverflow.com/questions/907390/how-can-i-tell-if-a-point-belongs-to-a-certain-line
        /// </summary>
        public bool IsBetween(BusStop stop1, BusStop stop2)
        {
            // If point is out of bounds, no need to do further checks
            if (X + SelectionFuzziness < Math.Min(stop1.X, stop2.X) 
                || Math.Max(stop1.X, stop2.X) < X - SelectionFuzziness)
            {
                return false;
            }

            if (Y + SelectionFuzziness < Math.Min(stop1.Y, stop2.Y) 
                || Math.Max(stop1.Y, stop2.Y) < Y - SelectionFuzziness)
            {
                return false;
            }

            double deltaX = stop1.X - stop2.X;
            double deltaY = stop1.Y - stop2.Y;

            // If the line is straight, the earlier boundary check is enough to determine that the point is on the line.
            // Also prevents division by zero exceptions.
            if (Math.Abs(deltaX) < Tolerance || Math.Abs(deltaY) < Tolerance)
            {
                return true;
            }

            var leftStop = stop1.X < stop2.X ? stop1 : stop2;

            double slope = deltaY / deltaX;
            double offset = leftStop.Y - leftStop.X * slope;
            double calculatedY = X * slope + offset;

            // Check calculated Y matches the points Y coord with some easing.
            bool lineContains = Y - SelectionFuzziness <= calculatedY && calculatedY <= Y + SelectionFuzziness;

            return lineContains;
        }
    }
}
