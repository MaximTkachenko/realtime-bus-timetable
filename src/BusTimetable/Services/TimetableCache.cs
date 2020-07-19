using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusTimetable.Interfaces;
using Models.Timetable;
using Orleans;

namespace BusTimetable.Services
{
    public interface ITimetableCache
    {
        IReadOnlyList<TimetableItem> GetTimetable(string busStopId);
    }

    public class TimetableCache : ITimetableCache, IDisposable
    {
        private readonly string[] _busStopIds;
        private readonly IClusterClient _clusterClient;
        private volatile Dictionary<string, IReadOnlyList<TimetableItem>> _cache;
        private readonly CancellationTokenSource _cts;

        public TimetableCache(IMetadataService metadata,
            IClusterClient clusterClient)
        {
            _busStopIds = metadata.GetMetadata().BusStops.Select(x => x.Id).ToArray();
            _clusterClient = clusterClient;
            _cache = new Dictionary<string, IReadOnlyList<TimetableItem>>();
            _cts = new CancellationTokenSource();

            Task.Run(() => CollectTimetables(_cts.Token), _cts.Token);
        }

        public IReadOnlyList<TimetableItem> GetTimetable(string busStopId)
        {
            return _cache.TryGetValue(busStopId, out var timetable)
                ? timetable
                : Array.Empty<TimetableItem>();
        }

        private async Task CollectTimetables(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var tasks = new Task<IReadOnlyList<TimetableItem>>[_busStopIds.Length];
                for (var i = 0; i < _busStopIds.Length; i++)
                {
                    tasks[i] = _clusterClient.GetGrain<IBusStop>(_busStopIds[i]).GetTimetable();
                }

                await Task.WhenAll(tasks);
                _cache = tasks.Select((timetableTask, index) => (BusStopId: _busStopIds[index], Timetable: timetableTask.Result))
                    .ToDictionary(x => x.BusStopId, x => x.Timetable);
                
                await Task.Delay(2000, stoppingToken);
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}
