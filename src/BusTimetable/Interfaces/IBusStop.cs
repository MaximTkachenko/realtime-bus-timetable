using System.Threading.Tasks;
using Orleans;

namespace BusTimetable.Interfaces
{
    public interface IBusStop : IGrainWithStringKey
    {
        Task UpdateBusArrival(string busId, int minutesBeforeArrival);
        Task<string[]> GetTimeTable();
    }
}
