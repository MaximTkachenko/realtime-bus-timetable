using System;
using System.Threading.Tasks;
using BusTimetable.Interfaces;
using Orleans;

namespace BusTimetable.Grains
{
    public class RouteGrain : Grain, IRoute
    {
        //needs bus route
        //bus stops tracking
        //time calculation

        public Task UpdateLocation(int x, int y)
        {
            //RegisterTimer()
            var busStop = GrainFactory.GetGrain<IBusStop>("");
            throw new NotImplementedException();
        }

        public override Task OnActivateAsync()
        {
            //todo load route details
            return base.OnActivateAsync();
        }
    }
}
