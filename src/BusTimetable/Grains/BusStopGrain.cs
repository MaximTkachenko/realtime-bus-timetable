using System.Collections.Generic;
using System.Threading.Tasks;
using BusTimetable.Interfaces;
using Microsoft.Extensions.Logging;
using Models.Timetable;
using Orleans;

namespace BusTimetable.Grains
{
    public class BusStopGrain : Grain, IBusStop
    {
        private readonly ILogger _logger;
        private string _stopId;
        private ITimetable _timetable;

        public BusStopGrain(ILogger<BusStopGrain> logger)
        {
            _logger = logger;
        }

        public Task UpdateRouteArrival(string routeId, double msBeforeArrival)
        {
            _logger.LogInformation("{stopId}: {duration} ms before {routeId}",
                _stopId, msBeforeArrival, routeId);

            _timetable.AddOrUpdate(routeId, msBeforeArrival);

            return Task.CompletedTask;
        }

        public override Task OnActivateAsync()
        {
            _stopId = this.GetPrimaryKeyString();
            _timetable = new NaiveTimetable();

            return base.OnActivateAsync();
        }

        public Task<IReadOnlyList<TimeTableItem>> GetTimeTable()
        {
            return Task.FromResult(_timetable.GetTimetable());
        }
    }
}
