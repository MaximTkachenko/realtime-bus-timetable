using System.Collections.Generic;
using System.Linq;

namespace Models.Timetable
{
    public class SmartTimetable : ITimetable
    {
        private readonly Dictionary<string, LinkedListNode<TimeTableItem>> _dictionary = new Dictionary<string, LinkedListNode<TimeTableItem>>();
        private readonly LinkedList<TimeTableItem> _items = new LinkedList<TimeTableItem>();

        public void AddOrUpdate(string routeId, double msBeforeArrival, Direction direction)
        {
            if (_dictionary.TryGetValue(routeId, out var item))
            {
                _items.Remove(item);
                item.Value.MsBeforeArrival = msBeforeArrival;
                item.Value.Direction = direction;

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
                var newItem = new TimeTableItem {RouteId = routeId, MsBeforeArrival = msBeforeArrival, Direction = direction};
                var found = Find(msBeforeArrival);
                var added = found == null
                    ? _items.AddLast(newItem)
                    : _items.AddBefore(found, newItem);
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

        private LinkedListNode<TimeTableItem> Find(double msBeforeArrival)
        {
            for (var node = _items.First; node != null; node = node.Next)
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
