using System.Threading.Tasks;
using Orleans;

namespace BusTimetable.Interfaces
{
    public interface IBusStop : IGrainWithStringKey
    {
        Task UpdateRouteArrival(string routeId, int msBeforeArrival);
        Task<string[]> GetTimeTable();
    }
}
