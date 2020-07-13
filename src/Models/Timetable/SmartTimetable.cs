using System.Collections.Generic;
using System.Linq;

namespace Models.Timetable
{
    public class SmartTimetable : ITimetable
    {
        private readonly Dictionary<string, LinkedListNode<TimeTableItem>> _dictionary = new Dictionary<string, LinkedListNode<TimeTableItem>>();
        private readonly LinkedList<TimeTableItem> _items = new LinkedList<TimeTableItem>();

        public void AddOrUpdate(string routeId, double msBeforeArrival)
        {
            if (_dictionary.TryGetValue(routeId, out var item))
            {
                _items.Remove(item);
                item.Value.MsBeforeArrival = msBeforeArrival;

                //todo we can do it more optimally
                var found = Find(msBeforeArrival);
                if (found == null)
                {
                    _items.AddLast(item);
                }
                else
                {
                    _items.AddBefore(found, item);
                }
            }
            else
            {
                var found = Find(msBeforeArrival);
                var added = found == null
                    ? _items.AddLast(new TimeTableItem {RouteId = routeId, MsBeforeArrival = msBeforeArrival})
                    : _items.AddBefore(found, new TimeTableItem { RouteId = routeId, MsBeforeArrival = msBeforeArrival });
                _dictionary.Add(routeId, added);
            }
        }

        public void Remove(string routeId)
        {
            if (_dictionary.TryGetValue(routeId, out var item))
            {
                _items.Remove(item);
                _dictionary.Remove(routeId);
            }
        }

        public IReadOnlyList<TimeTableItem> GetTimetable()
        {
            return _items.ToArray();
        }

        private LinkedListNode<TimeTableItem> Find(double msBeforeArrival, LinkedListNode<TimeTableItem> startNode = null, bool searchDown = true)
        {
            startNode = startNode ?? _items.First;
            for (var node = startNode; node != null; node = searchDown ? node.Next : node.Previous)
            {
                if (node.Value.MsBeforeArrival >= msBeforeArrival)
                {
                    return node;
                }
            }

            return null;
        }
    }
}
