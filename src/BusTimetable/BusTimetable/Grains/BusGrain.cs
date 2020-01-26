using System;
using System.Threading.Tasks;
using BusTimetable.Interfaces;
using Orleans;

namespace BusTimetable.Grains
{
    public class BusGrain : Grain, IBus
    {
        public Task UpdateLocation(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
