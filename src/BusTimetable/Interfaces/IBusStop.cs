using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Models.Timetable;
using Orleans;

namespace BusTimetable.Interfaces
{
    public interface IBusStop : IGrainWithStringKey
    {
        Task UpdateRouteArrival(string routeId, double msBeforeArrival, Direction direction);
        Task RemoveRouteArrival(string routeId);
        Task<IReadOnlyList<TimeTableItem>> GetTimetable();
    }
}
