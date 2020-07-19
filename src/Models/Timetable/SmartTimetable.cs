using System.Collections.Generic;
using System.Linq;

namespace Models.Timetable
{
    public class SmartTimetable : ITimetable
    {
        private readonly Dictionary<string, LinkedListNode<TimetableItem>> _dictionary = new Dictionary<string, LinkedListNode<TimetableItem>>();
        private readonly LinkedList<TimetableItem> _items = new LinkedList<TimetableItem>();

        public void AddOrUpdate(TimetableItem item)
        {
            if (_dictionary.TryGetValue(item.RouteId, out var existingItem))
            {
                _items.Remove(existingItem);
                existingItem.Value.MsBeforeArrival = item.MsBeforeArrival;
                existingItem.Value.Direction = item.Direction;
                existingItem.Value.UnixTimestamp = item.UnixTimestamp;

                //todo we can do it more optimally
                var found = Find(item.MsBeforeArrival);
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
                var found = Find(item.MsBeforeArrival);
                var added = found == null
                    ? _items.AddLast(item)
                    : _items.AddBefore(found, item);
                _dictionary.Add(item.RouteId, added);
            }
        }

        public void Clean(double timestampThreshold)
        {
            for (var node = _items.First; node != null; node = node.Next)
            {
                if (node.Value.UnixTimestamp < timestampThreshold)
                {
                    _items.Remove(node);
                }
            }
        }

        public IReadOnlyList<TimetableItem> GetTimetable()
        {
            return _items.ToArray();
        }

        private LinkedListNode<TimetableItem> Find(double msBeforeArrival)
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
