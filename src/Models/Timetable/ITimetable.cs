using System.Collections.Generic;

namespace Models.Timetable
{
    public interface ITimetable
    {
        void AddOrUpdate(string routeId, double msBeforeArrival, Direction direction);
        void Remove(string routeId);
        IReadOnlyList<TimeTableItem> GetTimetable();
    }
}
