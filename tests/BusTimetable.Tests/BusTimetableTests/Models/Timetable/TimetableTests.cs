using FluentAssertions;
using Models.Timetable;
using Xunit;

namespace BusTimetable.Tests.BusTimetableTests.Models.Timetable
{
    public class TimetableTests
    {
        [Fact]
        public void BasicTest()
        {
            var timetable = new NaiveTimetable();

            timetable.AddOrUpdate("a", 100);
            timetable.AddOrUpdate("b", 50);

            var items = timetable.GetTimetable();
            items.Count.Should().Be(2);
            items[0].RouteId.Should().Be("b");
            items[0].MsBeforeArrival.Should().Be(50);
            items[1].RouteId.Should().Be("a");
            items[1].MsBeforeArrival.Should().Be(100);

            timetable.AddOrUpdate("a", 20);
            timetable.AddOrUpdate("b", 40);

            items = timetable.GetTimetable();
            items.Count.Should().Be(2);
            items[0].RouteId.Should().Be("a");
            items[0].MsBeforeArrival.Should().Be(20);
            items[1].RouteId.Should().Be("b");
            items[1].MsBeforeArrival.Should().Be(40);
        }
    }
}
