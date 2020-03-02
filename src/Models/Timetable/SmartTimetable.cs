using System;
using System.Collections.Generic;

namespace Models.Timetable
{
    public class SmartTimetable : ITimetable
    {
        public void AddOrUpdate(string routeId, double msBeforeArrival)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<TimeTableItem> GetTimetable()
        {
            throw new NotImplementedException();
        }
    }
}
