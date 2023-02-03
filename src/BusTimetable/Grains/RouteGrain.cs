using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusTimetable.Services;
using Microsoft.Extensions.Logging;
using Models;
using Models.Timetable;
using Orleans;

namespace BusTimetable.Grains;

public interface IRoute : IGrainWithStringKey
{
    Task UpdateLocation(Location location);
}

public class RouteGrain : Grain, IRoute
{
    private readonly IMetadataService _metadata;
    private readonly ILogger<RouteGrain> _logger;

    private static readonly TimeSpan Threshold = TimeSpan.FromSeconds(4);

    private string _routeId;
    private Route _route;
    private BusStop[] _busStops;
    private double _timeSpentOnBusStop;

    private DateTime _lastUpdate = DateTime.UnixEpoch;
    private Direction _direction = Direction.Undefined;
    private Location _lastLocation = Location.NoLocation;
    private BusStop _nextBusStop;
    private double _lastClientUnixTimestamp;
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
        //location is out of order
        if (_lastClientUnixTimestamp > location.UnixTimestamp)
        {
            return;
        }

        _lastClientUnixTimestamp = location.UnixTimestamp;

        //location is the same
        var current = DateTime.UtcNow;
        if (location.Equals(_lastLocation))
        {
            _lastUpdate = current;
            return;
        }

        var busStopsInBetween = GetBusStopsInBetween(location);
        //location is out of route
        if (busStopsInBetween.BusStopIndex1 == -1
            && busStopsInBetween.BusStopIndex2 == -1)
        {
            _direction = Direction.Undefined;
            return;
        }

        var distanceFromBusStop1 = _busStops[busStopsInBetween.BusStopIndex1].GetDistance(location);
        var distanceFromBusStop2 = _busStops[busStopsInBetween.BusStopIndex2].GetDistance(location);

        //no previous location or it was received long time ago
        if (current - _lastUpdate > Threshold
            || _prevBusStopIndex1 == -1 && _prevBusStopIndex2 == -1)
        {
            //update prev state
            _prevBusStopIndex1 = busStopsInBetween.BusStopIndex1;
            _prevBusStopIndex2 = busStopsInBetween.BusStopIndex2;

            _prevDistanceFromBusStop1 = distanceFromBusStop1;
            _prevDistanceFromBusStop2 = distanceFromBusStop2;

            _lastLocation = location;
            _lastUpdate = current;

            return;
        }

        //previous and current locations are valid
        if (_prevBusStopIndex1 == busStopsInBetween.BusStopIndex1 
            && _prevBusStopIndex2 == busStopsInBetween.BusStopIndex2)
        {
            //we are still between the same bus stops

            if (_direction == Direction.There)
            {
                _direction = (distanceFromBusStop1 >= _prevDistanceFromBusStop1 || Math.Abs(distanceFromBusStop1 - _prevDistanceFromBusStop1) <= 3)
                             && (distanceFromBusStop2 <= _prevDistanceFromBusStop2 || Math.Abs(distanceFromBusStop2 - _prevDistanceFromBusStop2) <= 3)
                    ? Direction.There
                    : Direction.Back;
            }
            else
            {
                _direction = (distanceFromBusStop1 <= _prevDistanceFromBusStop1 || Math.Abs(distanceFromBusStop1 - _prevDistanceFromBusStop1) <= 3)
                             && (distanceFromBusStop2 >= _prevDistanceFromBusStop2 || Math.Abs(distanceFromBusStop2 - _prevDistanceFromBusStop2) <= 3)
                    ? Direction.Back
                    : Direction.There;
            }
        }
        else
        {
            //we are on a the next segment of route

            _direction = busStopsInBetween.BusStopIndex1 > _prevBusStopIndex1
                         && busStopsInBetween.BusStopIndex2 > _prevBusStopIndex2
                ? Direction.There
                : Direction.Back;
        }

        //update prev state
        _prevBusStopIndex1 = busStopsInBetween.BusStopIndex1;
        _prevBusStopIndex2 = busStopsInBetween.BusStopIndex2;

        _prevDistanceFromBusStop1 = distanceFromBusStop1;
        _prevDistanceFromBusStop2 = distanceFromBusStop2;

        _lastLocation = location;
        _lastUpdate = current;

        if (_routeId == "repeat-obey-1")
        {
            _logger.LogInformation(_direction == Direction.There
                ? $"{_direction}: {busStopsInBetween.BusStopIndex1} -> {busStopsInBetween.BusStopIndex2}"
                : $"{_direction}: {busStopsInBetween.BusStopIndex2} -> {busStopsInBetween.BusStopIndex1}");
        }

        await NotifyBusStops(busStopsInBetween);
    }

    public override Task OnActivateAsync(CancellationToken _)
    {
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

        return base.OnActivateAsync(_);
    }

    private Task NotifyBusStops((int BusStopIndex1, int BusStopIndex2) busStopsInBetween)
    {
        var nextBusStop = _direction == Direction.There 
            ? _busStops[busStopsInBetween.BusStopIndex2] 
            : _busStops[busStopsInBetween.BusStopIndex1];
        _nextBusStop = nextBusStop;
        var distance = nextBusStop.GetDistance(_lastLocation);
        var duration = distance / _route.Velocity;

        var tasks = new Task[_direction == Direction.There 
            ? _busStops.Length - busStopsInBetween.BusStopIndex2 
            : busStopsInBetween.BusStopIndex2];
        for (var i = _direction == Direction.There ? busStopsInBetween.BusStopIndex2 : busStopsInBetween.BusStopIndex1;
            _direction == Direction.There ? i < _busStops.Length : i > -1;
            i = _direction == Direction.There ? i + 1 : i - 1)
        {
            if (_direction == Direction.There && i > busStopsInBetween.BusStopIndex2
                || _direction == Direction.Back && i < busStopsInBetween.BusStopIndex1)
            {
                duration += _timeSpentOnBusStop + _route.Path[i].Duration;
            }
            nextBusStop = _busStops[i];
            var busStopGrain = GrainFactory.GetGrain<IBusStop>(nextBusStop.Id);

            var taskIndex = _direction == Direction.There
                ? i - busStopsInBetween.BusStopIndex2
                : i;
            var item = new TimetableItem
            {
                RouteId = _routeId, 
                MsBeforeArrival = Math.Truncate(duration / 1000),
                Direction = _direction,
                UnixTimestamp = DateTime.UtcNow.ToUnixTimestamp()
            };
            tasks[taskIndex] = busStopGrain.UpdateRouteArrival(item);
        }
        return Task.WhenAll(tasks);
    }

    private (int BusStopIndex1, int BusStopIndex2) GetBusStopsInBetween(Location location)
    {
        for (var i = 0; i < _busStops.Length - 1; i++)
        {
            if (location.IsBetween(_busStops[i], _busStops[i + 1]))
            {
                return (i, i + 1);
            }
        }

        return (-1, -1);
    }
}