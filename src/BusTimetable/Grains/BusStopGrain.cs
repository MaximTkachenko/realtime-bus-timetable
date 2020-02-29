using System;
using System.Threading.Tasks;
using BusTimetable.Interfaces;
using Microsoft.Extensions.Logging;
using Models;
using Orleans;

namespace BusTimetable.Grains
{
    public class BusStopGrain : Grain, IBusStop
    {
        private readonly ILogger _logger;
        private string _stopId;

        //should contain timetable
        //updates bus arrival according to messages from bus grain

        public BusStopGrain(ILogger<BusStopGrain> logger)
        {
            _logger = logger;
        }

        public Task UpdateRouteArrival(string routeId, float msBeforeArrival)
        {
            //RegisterTimer()
            _logger.LogInformation("{stopId}: {routeId} will arrive after {duration} ms",
                _stopId, routeId, msBeforeArrival);
            return Task.CompletedTask;
        }

        public override Task OnActivateAsync()
        {
            _stopId = this.GetPrimaryKeyString();

            return base.OnActivateAsync();
        }

        public Task<TimeTableItem[]> GetTimeTable()
        {
            return Task.FromResult(Array.Empty<TimeTableItem>());
        }
    }
}
