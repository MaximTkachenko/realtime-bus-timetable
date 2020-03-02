using FluentAssertions;
using Models;
using Xunit;

namespace BusTimetable.Tests.BusTimetableTests.Models
{
    public class LocationTests
    {
        [Theory]
        [InlineData(1f, 1f, false)]
        [InlineData(7.5f, 7.5f, true)]
        public void IsBetween_LocationOnLineBetweenStops_ReturnsTrue(double x, double y, bool result)
        {
            //todo add more tests
            var stop1 = new BusStop {X = 5, Y = 5};
            var stop2 = new BusStop { X = 10, Y = 10 };
            var location = new Location {X = x, Y = y};

            location.IsBetween(stop1, stop2).Should().Be(result);
        }
    }
}
