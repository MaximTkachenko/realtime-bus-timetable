using System.Threading.Tasks;
using BusTimetable.Models;
using Orleans;

namespace BusTimetable.Interfaces
{
    public interface IBusStop : IGrainWithStringKey
    {
        Task UpdateRouteArrival(string routeId, float msBeforeArrival);
        Task<TimeTableItem[]> GetTimeTable();
    }
}
