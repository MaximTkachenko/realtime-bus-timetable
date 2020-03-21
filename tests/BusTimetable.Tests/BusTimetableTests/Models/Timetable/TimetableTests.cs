using System.Collections.Generic;
using FluentAssertions;
using Models.Timetable;
using Xunit;

namespace BusTimetable.Tests.BusTimetableTests.Models.Timetable
{
    public class TimetableTests
    {
        [Theory, MemberData(nameof(Timetables))]
        public void AddOrUpdate_BasicTest(ITimetable timetable)
        {
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

        [Theory, MemberData(nameof(Timetables))]
        public void Remove_BasicTest(ITimetable timetable)
        {
            timetable.AddOrUpdate("a", 100);
            timetable.AddOrUpdate("b", 50);
            timetable.Remove("a");

            var items = timetable.GetTimetable();
            items.Count.Should().Be(1);
            items[0].RouteId.Should().Be("b");
            items[0].MsBeforeArrival.Should().Be(50);
        }

        public static IEnumerable<object[]> Timetables
        {
            get
            {
                return new []
                {
                    new object[] { new NaiveTimetable() },
                    //new object[] { new SmartTimetable() }
                };
            }
        }
    }
}
