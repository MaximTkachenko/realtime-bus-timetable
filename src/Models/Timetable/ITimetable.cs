using System.Collections.Generic;

namespace Models.Timetable
{
    public interface ITimetable
    {
        void AddOrUpdate(string routeId, double msBeforeArrival);
        IReadOnlyList<TimeTableItem> GetTimetable();
    }
}
