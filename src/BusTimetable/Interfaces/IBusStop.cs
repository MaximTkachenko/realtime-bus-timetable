using System.Threading.Tasks;
using Models;
using Orleans;

namespace BusTimetable.Interfaces
{
    public interface IBusStop : IGrainWithStringKey
    {
        Task UpdateRouteArrival(string routeId, float msBeforeArrival);
        Task<TimeTableItem[]> GetTimeTable();
    }
}
