using System;
using System.Text.Json.Serialization;
using Orleans;

namespace Models;

[GenerateSerializer]
public class Location
{
    public static Location NoLocation => new() { X = -1, Y = -1 };

    private const double SelectionFuzziness = 3;
    private const double Tolerance = 0.1f;

    [Id(0), JsonPropertyName("x")] public double X { get; set; }
    [Id(1), JsonPropertyName("y")] public double Y { get; set; }
    [Id(2), JsonPropertyName("unixTimestamp")] public double UnixTimestamp { get; set; }

    public bool Equals(Location other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y);
    }

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

        // Calculate equation for the line: y = x * slope + offset
        // And then calculate Y for point's X
        double slope = deltaY / deltaX;
        double offset = leftStop.Y - leftStop.X * slope;
        double calculatedY = X * slope + offset;

        // Check calculated Y matches the point's Y with some easing.
        bool isBetween = Y - SelectionFuzziness <= calculatedY && calculatedY <= Y + SelectionFuzziness;

        return isBetween;
    }
}