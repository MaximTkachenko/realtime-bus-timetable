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

            //todo probably I can keep a last N locations
            _currentLocation = location;

            var nextBusStop = GetNextBusStop();
            var distance = nextBusStop.GetDistance(_currentLocation);
            var duration = distance / _route.Velocity;

            var busStop = GrainFactory.GetGrain<IBusStop>(nextBusStop.Id);

            await busStop.UpdateRouteArrival(_routeId, duration);
            //todo notify rest bus stops
        }

        public override Task OnActivateAsync()
        {
            _currentLocation = Location.NoLocation;
            _routeId = this.GetPrimaryKeyString();

            var metadata = _metadata.GetMetadata();
            _route = metadata.Routes.First(x => x.Id == _routeId);
            _busStops = new BusStop[_route.Path.Length];
            for (int i = 0; i < _route.Path.Length; i++)
            {
                _busStops[i] = metadata.BusStops[_route.Path[i].BusStopIndex];
            }
            
            return base.OnActivateAsync();
        }

        private BusStop GetNextBusStop()
        {
            for (var i = 0; i < _busStops.Length - 1; i++)
            {
                if (_currentLocation.IsBetween(_busStops[i], _busStops[i + 1]))
                {
                    return _busStops[i + 1];
                }
            }

            return BusStop.NoStop;
        }
    }
}
