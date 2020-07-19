using System.Collections.Generic;
using System.Linq;

namespace Models.Timetable
{
    public class NaiveTimetable : ITimetable
    {
        private List<TimetableItem> _items = new List<TimetableItem>();

        public void AddOrUpdate(TimetableItem item)
        {
            _items.RemoveAll(x => x.RouteId == item.RouteId);
            _items.Add(item);
            _items = _items.OrderBy(x => x.MsBeforeArrival).ToList();
        }

        public void Clean(double timestampThreshold)
        {
            _items.RemoveAll(x => x.UnixTimestamp < timestampThreshold);
        }

        public IReadOnlyList<TimetableItem> GetTimetable()
        {
            return _items;
        }
    }
}
