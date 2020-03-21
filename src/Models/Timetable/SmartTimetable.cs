using System;
using System.Collections.Generic;

namespace Models.Timetable
{
    public class SmartTimetable : ITimetable
    {
        private Dictionary<string, TimeTableItem> _dictionary = new Dictionary<string, TimeTableItem>();
        private LinkedList<TimeTableItem> _items = new LinkedList<TimeTableItem>();

        public void AddOrUpdate(string routeId, double msBeforeArrival)
        {
            throw new NotImplementedException();
        }

        public void Remove(string routeId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<TimeTableItem> GetTimetable()
        {
            throw new NotImplementedException();
        }
    }
}
