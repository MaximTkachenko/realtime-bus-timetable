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
            throw new NotImplementedException();
        }
    }
}
