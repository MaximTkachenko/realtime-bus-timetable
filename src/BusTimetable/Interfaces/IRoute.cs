using System.Threading.Tasks;
using Models;
using Orleans;

namespace BusTimetable.Interfaces
{
    public interface IRoute : IGrainWithStringKey
    {
        Task UpdateLocation(Location location);
    }
}
