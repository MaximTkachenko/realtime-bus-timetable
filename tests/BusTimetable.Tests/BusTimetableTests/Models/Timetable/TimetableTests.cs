using System;
using System.Collections.Generic;
using FluentAssertions;
using Models;
using Models.Timetable;
using Xunit;

namespace BusTimetable.Tests.BusTimetableTests.Models.Timetable;

public class TimetableTests
{
    [Theory, MemberData(nameof(Timetables))]
    public void AddOrUpdate_BasicTest(ITimetable timetable)
    {
        timetable.AddOrUpdate(new TimetableItem("a", 100, Direction.There, DateTime.UtcNow.ToUnixTimestamp()));
        timetable.AddOrUpdate(new TimetableItem("b", 50, Direction.There, DateTime.UtcNow.ToUnixTimestamp()));

        var items = timetable.GetTimetable();
        items.Count.Should().Be(2);
        items[0].RouteId.Should().Be("b");
        items[0].MsBeforeArrival.Should().Be(50);
        items[1].RouteId.Should().Be("a");
        items[1].MsBeforeArrival.Should().Be(100);

        timetable.AddOrUpdate(new TimetableItem("a", 20, Direction.There, DateTime.UtcNow.ToUnixTimestamp()));
        timetable.AddOrUpdate(new TimetableItem("b", 40, Direction.There, DateTime.UtcNow.ToUnixTimestamp()));

        items = timetable.GetTimetable();
        items.Count.Should().Be(2);
        items[0].RouteId.Should().Be("a");
        items[0].MsBeforeArrival.Should().Be(20);
        items[1].RouteId.Should().Be("b");
        items[1].MsBeforeArrival.Should().Be(40);
    }

    //todo ad tests for clean

    public static IEnumerable<object[]> Timetables
    {
        get
        {
            return new []
            {
                new object[] { new NaiveTimetable() },
                new object[] { new SmartTimetable() }
            };
        }
    }
}