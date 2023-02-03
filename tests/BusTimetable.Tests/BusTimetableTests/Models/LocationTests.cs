using FluentAssertions;
using Models;
using Xunit;

namespace BusTimetable.Tests.BusTimetableTests.Models;

public class LocationTests
{
    [Theory]
    [InlineData(10f, 10f, 15f, 15f, 1f, 1f, false)] //before line
    [InlineData(10f, 10f, 15f, 15f, 20f, 20f, false)] // after line
    [InlineData(10f, 10f, 15f, 15f, 12.5f, 16f, false)] //under the line, more than fuzziness
    [InlineData(10f, 10f, 15f, 15f, 16f, 12.5f, false)] //above the line, more than fuzziness
    [InlineData(10f, 10f, 15f, 15f, 12.5f, 15f, true)] //under the line, less than fuzziness
    [InlineData(10f, 10f, 15f, 15f, 15f, 12.5f, true)] //above the line, less than fuzziness
    [InlineData(10f, 10f, 15f, 15f, 12.5f, 12.5f, true)] //on line
    [InlineData(10f, 10f, 15f, 15f, 10f, 10f, true)] //start point
    [InlineData(10f, 10f, 15f, 15f, 15f, 15f, true)] //end point
    public void IsBetween_DifferentCases_HandledProperly(double stop1X, double stop1Y,
        double stop2X, double stop2Y,
        double x, double y, bool result)
    {
        var stop1 = new BusStop {X = stop1X, Y = stop1Y};
        var stop2 = new BusStop { X = stop2X, Y = stop2Y };
        var location = new Location {X = x, Y = y};

        location.IsBetween(stop1, stop2).Should().Be(result);
    }
}