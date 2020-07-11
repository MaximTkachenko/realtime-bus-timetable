using System;
using System.Linq;
using System.Threading.Tasks;
using BusTimetable.Interfaces;
using BusTimetable.Services;
using Microsoft.Extensions.Logging;
using Models;
using Orleans;

namespace BusTimetable.Grains
{
    public class RouteGrain : Grain, IRoute
    {
        private readonly IMetadataService _metadata;
        private readonly ILogger<RouteGrain> _logger;

        private string _routeId;
        private Route _route;
        private BusStop[] _busStops;
        private Location _currentLocation;
        private BusStop _nextBusStop;
        private double _timeSpentOnBusStop;

        private int _prevBusStopIndex1 = -1;
        private int _prevBusStopIndex2 = -1;
        private double _prevDistanceFromBusStop1;
        private double _prevDistanceFromBusStop2;

        public RouteGrain(IMetadataService metadata, 
            ILogger<RouteGrain> logger)
        {
            _metadata = metadata;
            _logger = logger;
        }

        public async Task UpdateLocation(Location location)
        {
            if (_currentLocation.Equals(location))
            {
                return;
            }

            //todo compare last timestamp and current timestamp, if difference is too big - ignore
            //todo define state
            //todo calculate distance, notify bus stop


            //todo probably I can keep last N locations
            _currentLocation = location;

            var busStopsInBetween = GetBusStopsInBetween();
            if (_routeId == "method-first-0")
            {
                _logger.LogInformation($"{busStopsInBetween.BusStopIndex1} - {busStopsInBetween.BusStopIndex2}");
            }

            if (busStopsInBetween.BusStopIndex2 < 0)
            {
                //todo need to remove from the last bus stop
                return;
            }

            var nextBusStop = _busStops[busStopsInBetween.BusStopIndex2];
            if (!_nextBusStop.Equals(nextBusStop))
            {
                var nextBusStopGrainOld = GrainFactory.GetGrain<IBusStop>(_nextBusStop.Id);
                await nextBusStopGrainOld.RemoveRouteArrival(_routeId);
            }

            if (busStopsInBetween.BusStopIndex1 != -1
                && busStopsInBetween.BusStopIndex2 != -1)
            {
                if (_prevBusStopIndex1 == busStopsInBetween.BusStopIndex1
                    && _prevBusStopIndex2 == busStopsInBetween.BusStopIndex2)
                {
                    var distanceFromBusStop1 = _busStops[busStopsInBetween.BusStopIndex1].GetDistance(_currentLocation);
                    var distanceFromBusStop2 = _busStops[busStopsInBetween.BusStopIndex2].GetDistance(_currentLocation);

                    if (_routeId == "method-first-0")
                    {
                        var state = distanceFromBusStop1 >= _prevDistanceFromBusStop1 && distanceFromBusStop2 <= _prevDistanceFromBusStop2 
                            ? "down" 
                            : "up";
                        _logger.LogInformation($"{state}: {distanceFromBusStop1} >= {_prevDistanceFromBusStop1}");
                    }

                    _prevDistanceFromBusStop1 = distanceFromBusStop1;
                    _prevDistanceFromBusStop2 = distanceFromBusStop2;
                }
                else
                {
                    _prevDistanceFromBusStop1 = _busStops[busStopsInBetween.BusStopIndex1].GetDistance(_currentLocation);
                    _prevDistanceFromBusStop2 = _busStops[busStopsInBetween.BusStopIndex2].GetDistance(_currentLocation);

                    if (_prevBusStopIndex1 != -1
                        && _prevBusStopIndex2 != 1)
                    {
                        if (_routeId == "method-first-0")
                        {
                            var state = busStopsInBetween.BusStopIndex1 > _prevBusStopIndex1 && busStopsInBetween.BusStopIndex2 > _prevBusStopIndex2
                                ? "down"
                                : "up";
                            _logger.LogInformation($"{state}: moved ");
                        }
                    }
                }

                _prevBusStopIndex1 = busStopsInBetween.BusStopIndex1;
                _prevBusStopIndex2 = busStopsInBetween.BusStopIndex2;
            }

            _nextBusStop = nextBusStop;
            var distance = nextBusStop.GetDistance(_currentLocation);
            var duration = distance / _route.Velocity;

            var tasks = new Task[_busStops.Length - busStopsInBetween.BusStopIndex2];
            for (var i = busStopsInBetween.BusStopIndex2; i < _busStops.Length; i++)
            {
                if (i > busStopsInBetween.BusStopIndex2)
                {
                    duration += _timeSpentOnBusStop + _route.Path[i].Duration;
                }
                nextBusStop = _busStops[i];
                var busStopGrain = GrainFactory.GetGrain<IBusStop>(nextBusStop.Id);
                tasks[i - busStopsInBetween.BusStopIndex2] = busStopGrain.UpdateRouteArrival(_routeId, Math.Truncate(duration / 1000));
            }
            await Task.WhenAll(tasks);
        }

        public override Task OnActivateAsync()
        {
            _currentLocation = Location.NoLocation;
            _routeId = this.GetPrimaryKeyString();

            var metadata = _metadata.GetMetadata();
            _timeSpentOnBusStop = metadata.TimeSpentOnBusStopMs;
            _route = metadata.Routes.First(x => x.Id == _routeId);
            _busStops = new BusStop[_route.Path.Length];
            for (int i = 0; i < _route.Path.Length; i++)
            {
                _busStops[i] = metadata.BusStops[_route.Path[i].BusStopIndex];
            }

            _nextBusStop = _busStops[0];

            return base.OnActivateAsync();
        }

        private (int BusStopIndex1, int BusStopIndex2) GetBusStopsInBetween()
        {
            for (var i = 0; i < _busStops.Length - 1; i++)
            {
                if (_currentLocation.IsBetween(_busStops[i], _busStops[i + 1]))
                {
                    return (i, i + 1);
                }
            }

            return (-1, -1);
        }
    }
}
