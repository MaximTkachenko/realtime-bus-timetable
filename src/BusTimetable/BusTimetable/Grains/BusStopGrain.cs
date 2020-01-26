using System;
using System.Threading.Tasks;
using BusTimetable.Interfaces;
using Orleans;

namespace BusTimetable.Grains
{
    public class BusStopGrain : Grain, IBusStop
    {
        public Task UpdateBusArrival(string busId, int minutesBeforeArrival)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> GetTimeTableAsync()
        {
            throw new NotImplementedException();
        }
    }
}
