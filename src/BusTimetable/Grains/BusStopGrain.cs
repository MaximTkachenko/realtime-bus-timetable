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

        public Task RemoveRouteArrival(string routeId)
        {
            _timetable.Remove(routeId);
            return Task.CompletedTask;
        }

        public override Task OnActivateAsync()
        {
            _timetable = new NaiveTimetable();
            return base.OnActivateAsync();
        }

        //todo this method should be optimized using:
        // - https://dotnet.github.io/orleans/Documentation/grains/stateless_worker_grains.html
        // - https://github.com/dotnet/orleans/issues/5283#issuecomment-450977696
        // - https://github.com/dotnet/orleans/issues/3841
        // - https://stackoverflow.com/questions/48145496/msr-orleans-how-to-create-a-reader-writer-grain-with-parallel-reads/48146722#48146722
        public Task<IReadOnlyList<TimeTableItem>> GetTimeTable()
        {
            return Task.FromResult(_timetable.GetTimetable());
        }
    }
}
