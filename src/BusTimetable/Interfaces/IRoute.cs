using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;

namespace BusTimetable.Interfaces
{
    public interface IRoute : IGrainWithStringKey
    {
        Task UpdateLocation(int x, int y);
    }
}
