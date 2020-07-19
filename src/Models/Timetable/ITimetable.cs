using System.Collections.Generic;

namespace Models.Timetable
{
    public interface ITimetable
    {
        void AddOrUpdate(TimetableItem item);
        void Clean(double timestampThreshold);
        IReadOnlyList<TimetableItem> GetTimetable();
    }
}
