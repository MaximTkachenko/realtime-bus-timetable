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

        public Task UpdateRouteArrival(string routeId, int msBeforeArrival)
        {
            //RegisterTimer()
            return Task.CompletedTask;
        }

        public override Task OnActivateAsync()
        {
            //todo load data regarding stop

            return base.OnActivateAsync();
        }

        public Task<string[]> GetTimeTable()
        {
            return Task.FromResult(Array.Empty<string>());
        }
    }
}
