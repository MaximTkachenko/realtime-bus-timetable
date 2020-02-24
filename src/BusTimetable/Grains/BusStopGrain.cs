using System;
using System.Threading.Tasks;
using BusTimetable.Interfaces;
using Orleans;

namespace BusTimetable.Grains
{
    public class BusStopGrain : Grain, IBusStop
    {
        //should contain timetable
        //updates bus arrival according to messages from bus grain

        public Task UpdateBusArrival(string busId, int minutesBeforeArrival)
        {
            //RegisterTimer()
            return Task.CompletedTask;
        }

        public Task<string[]> GetTimeTable()
        {
            return Task.FromResult(Array.Empty<string>());
        }
    }
}
