using System.Threading.Tasks;
using Orleans;

namespace BusTimetable.Interfaces
{
    public interface IRoute : IGrainWithStringKey
    {
        Task UpdateLocation(int x, int y);
    }
}
