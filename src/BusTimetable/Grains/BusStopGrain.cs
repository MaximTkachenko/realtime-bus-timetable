using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusTimetable.Interfaces;
using Microsoft.Extensions.Logging;
using Models;
using Models.Timetable;
using Orleans;

namespace BusTimetable.Grains
{
    public class BusStopGrain : Grain, IBusStop
    {
        private readonly ILogger _logger;

        private ITimetable _timetable;
        
        private static readonly TimeSpan CleanupInterval = TimeSpan.FromSeconds(3);

        public BusStopGrain(ILogger<RouteGrain> logger)
        {
            _logger = logger;
        }

        public Task UpdateRouteArrival(TimetableItem item)
        {
            _timetable.AddOrUpdate(item);
            return Task.CompletedTask;
        }

        public override Task OnActivateAsync(CancellationToken _)
        {
            _timetable = new NaiveTimetable();

            RegisterTimer(o =>
            {
                _timetable.Clean(DateTime.UtcNow.AddSeconds(-4).ToUnixTimestamp());
                return Task.CompletedTask;
            }, null, CleanupInterval, CleanupInterval);

            return base.OnActivateAsync(_);
        }

        //todo this method should be optimized using:
        // - https://dotnet.github.io/orleans/Documentation/grains/stateless_worker_grains.html
        // - https://github.com/dotnet/orleans/issues/5283#issuecomment-450977696
        // - https://github.com/dotnet/orleans/issues/3841
        // - https://stackoverflow.com/questions/48145496/msr-orleans-how-to-create-a-reader-writer-grain-with-parallel-reads/48146722#48146722
        public Task<IReadOnlyList<TimetableItem>> GetTimetable()
        {
            return Task.FromResult(_timetable.GetTimetable());
        }
    }
}
