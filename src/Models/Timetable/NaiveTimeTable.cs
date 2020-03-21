using System.Collections.Generic;
using System.Linq;

namespace Models.Timetable
{
    public class NaiveTimetable : ITimetable
    {
        private List<TimeTableItem> _items = new List<TimeTableItem>();

        public void AddOrUpdate(string routeId, double msBeforeArrival)
        {
            _items.RemoveAll(x => x.RouteId == routeId);
            _items.Add(new TimeTableItem{RouteId = routeId, MsBeforeArrival = msBeforeArrival});
            _items = _items.OrderBy(x => x.MsBeforeArrival).ToList();
        }

        public void Remove(string routeId)
        {
            _items.RemoveAll(x => x.RouteId == routeId);
        }

        public IReadOnlyList<TimeTableItem> GetTimetable()
        {
            return _items;
        }
    }
}
