using System;
using System.Linq;
using System.Threading.Tasks;
using BusTimetable.Interfaces;
using BusTimetable.Services;
using Models;
using Orleans;

namespace BusTimetable.Grains
{
    public class RouteGrain : Grain, IRoute
    {
        private readonly IMetadataService _metadata;

        private string _routeId;
        private Route _route;
        private BusStop[] _busStops;
        private Location _currentLocation;
        private BusStop _nextBusStop;
        private double _timeSpentOnBusStop;

        public RouteGrain(IMetadataService metadata)
        {
            _metadata = metadata;
        }

        public async Task UpdateLocation(Location location)
        {
            if (_currentLocation.Equals(location))
            {
                return;
            }

            //todo probably I can keep last N locations
            _currentLocation = location;

            var nextBusStopIndex = GetNextBusStopIndex();
            if (nextBusStopIndex < 0)
            {
                return;
            }

            var nextBusStop = _busStops[nextBusStopIndex];
            if (!_nextBusStop.Equals(nextBusStop))
            {
                var nextBusStopGrainOld = GrainFactory.GetGrain<IBusStop>(_nextBusStop.Id);
                await nextBusStopGrainOld.RemoveRouteArrival(_routeId);
            }

            _nextBusStop = nextBusStop;
            var distance = _nextBusStop.GetDistance(_currentLocation);
            var duration = distance / _route.Velocity;

            var tasks = new Task[_busStops.Length - nextBusStopIndex];
            for (var i = nextBusStopIndex; i < _busStops.Length; i++)
            {
                if (i > nextBusStopIndex)
                {
                    duration += _timeSpentOnBusStop + _route.Path[i].Duration;
                }
                _nextBusStop = _busStops[i];
                var busStopGrain = GrainFactory.GetGrain<IBusStop>(_nextBusStop.Id);
                tasks[i - nextBusStopIndex] = busStopGrain.UpdateRouteArrival(_routeId, Math.Truncate(duration / 1000));
            }
            await Task.WhenAll(tasks);
        }

        public override Task OnActivateAsync()
        {
            _currentLocation = Location.NoLocation;
            _routeId = this.GetPrimaryKeyString();

            var metadata = _metadata.GetMetadata();
            _timeSpentOnBusStop = metadata.TimeSpentOnBusStop;
            _route = metadata.Routes.First(x => x.Id == _routeId);
            _busStops = new BusStop[_route.Path.Length];
            for (int i = 0; i < _route.Path.Length; i++)
            {
                _busStops[i] = metadata.BusStops[_route.Path[i].BusStopIndex];
            }

            _nextBusStop = _busStops[0];

            return base.OnActivateAsync();
        }

        private int GetNextBusStopIndex()
        {
            for (var i = 0; i < _busStops.Length - 1; i++)
            {
                if (_currentLocation.IsBetween(_busStops[i], _busStops[i + 1]))
                {
                    return i + 1;
                }
            }

            return -1;
        }
    }
}
