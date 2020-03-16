using System.Collections.Generic;
using System.Threading.Tasks;
using BusTimetable.Interfaces;
using Models.Timetable;
using Orleans;

namespace BusTimetable.Grains
{
    public class BusStopGrain : Grain, IBusStop
    {
        private ITimetable _timetable;

        public Task UpdateRouteArrival(string routeId, double msBeforeArrival)
        {
            _timetable.AddOrUpdate(routeId, msBeforeArrival);
            return Task.CompletedTask;
        }

        public override Task OnActivateAsync()
        {
            _timetable = new NaiveTimetable();
            return base.OnActivateAsync();
        }

        public Task<IReadOnlyList<TimeTableItem>> GetTimeTable()
        {
            return Task.FromResult(_timetable.GetTimetable());
        }
    }
}
