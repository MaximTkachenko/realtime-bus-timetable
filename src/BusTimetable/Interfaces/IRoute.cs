using System.Threading.Tasks;
using BusTimetable.Models;
using Orleans;

namespace BusTimetable.Interfaces
{
    public interface IRoute : IGrainWithStringKey
    {
        Task UpdateLocation(Location location);
    }
}
