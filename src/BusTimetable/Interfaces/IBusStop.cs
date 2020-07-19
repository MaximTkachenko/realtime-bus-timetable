using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Timetable;
using Orleans;

namespace BusTimetable.Interfaces
{
    public interface IBusStop : IGrainWithStringKey
    {
        Task UpdateRouteArrival(TimetableItem item);
        Task<IReadOnlyList<TimetableItem>> GetTimetable();
    }
}
