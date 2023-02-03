using System;

namespace Models;

public class RouteSection
{
    private const double SelectionFuzziness = 3;
    private const double Tolerance = 0.1f;

    public static RouteSection NoRouteSection = new RouteSection(BusStop.NoBusStop, BusStop.NoBusStop);

    public RouteSection(BusStop busStop1, BusStop busStop2)
    {
        BusStop1 = busStop1 ?? throw new NullReferenceException(nameof(busStop1));
        BusStop2 = busStop2 ?? throw new NullReferenceException(nameof(busStop2));
    }

    public BusStop BusStop1 { get; }
    public BusStop BusStop2 { get; }

    public bool IsValid => this != NoRouteSection;

    /// <summary>
    /// Based on https://stackoverflow.com/questions/907390/how-can-i-tell-if-a-point-belongs-to-a-certain-line
    /// </summary>
    public bool Contains(Location location)
    {
        // If point is out of bounds, no need to do further checks
        if (location.X + SelectionFuzziness < Math.Min(BusStop1.X, BusStop2.X)
            || Math.Max(BusStop1.X, BusStop2.X) < location.X - SelectionFuzziness)
        {
            return false;
        }

        if (location.Y + SelectionFuzziness < Math.Min(BusStop1.Y, BusStop2.Y)
            || Math.Max(BusStop1.Y, BusStop2.Y) < location.Y - SelectionFuzziness)
        {
            return false;
        }

        double deltaX = BusStop1.X - BusStop2.X;
        double deltaY = BusStop1.Y - BusStop2.Y;

        // If the line is straight, the earlier boundary check is enough to determine that the point is on the line.
        // Also prevents division by zero exceptions.
        if (Math.Abs(deltaX) < Tolerance || Math.Abs(deltaY) < Tolerance)
        {
            return true;
        }

        var leftStop = BusStop1.X < BusStop2.X ? BusStop1 : BusStop2;

        // Calculate equation for the line: y = x * slope + offset
        // And then calculate Y for point's X
        double slope = deltaY / deltaX;
        double offset = leftStop.Y - leftStop.X * slope;
        double calculatedY = location.X * slope + offset;

        // Check calculated Y matches the point's Y with some easing.
        bool isBetween = location.Y - SelectionFuzziness <= calculatedY && calculatedY <= location.Y + SelectionFuzziness;

        return isBetween;
    }
}