using System.Threading.Tasks;
using BusTimetable.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;

namespace BusTimetable.Grains
{
    public class RouteGrain : Grain, IRoute
    {
        private readonly ILogger _logger;

        public RouteGrain(ILogger<RouteGrain> logger)
        {
            _logger = logger;
        }

        //needs bus route
        //bus stops tracking
        //time calculation

        public Task UpdateLocation(float x, float y)
        {
            //RegisterTimer()
            //var busStop = GrainFactory.GetGrain<IBusStop>("");
            _logger.LogInformation("{id} has new x={x}, y={y}", this.GetPrimaryKeyString(), x, y);
            return Task.CompletedTask;
        }

        public override Task OnActivateAsync()
        {
            //todo load route details
            return base.OnActivateAsync();
        }
    }
}
