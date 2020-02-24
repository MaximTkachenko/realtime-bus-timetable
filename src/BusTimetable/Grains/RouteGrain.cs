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
        private readonly ILogger _logger;
        private readonly IMetadataService _metadata;
        private Route _route;
        private BusStop[] _busStops;

        public RouteGrain(ILogger<RouteGrain> logger,
            IMetadataService metadata)
        {
            _logger = logger;
            _metadata = metadata;
        }

        //needs bus route
        //bus stops tracking
        //time calculation

        public Task UpdateLocation(float x, float y)
        {
            _logger.LogInformation("{id} has new x={x}, y={y}", this.GetPrimaryKeyString(), x, y);
            //var busStop = GrainFactory.GetGrain<IBusStop>("");
            return Task.CompletedTask;
        }

        public override Task OnActivateAsync()
        {
            var id = this.GetPrimaryKeyString();
            var metadata = _metadata.GetMetadata();
            _route = metadata.Routes.First(x => x.Id == id);
            _busStops = metadata.BusStops;
            return base.OnActivateAsync();
        }
    }
}
