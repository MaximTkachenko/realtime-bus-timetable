using System.IO;
using System.Reflection;
using System.Text.Json;
using Models;

namespace BusTimetable.Services
{
    public class MetadataService : IMetadataService
    {
        private readonly Root _metadata; 
        private const decimal Velocity = 0.08m; //pixels in milliseconds

        public MetadataService()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "metadata.json");
            _metadata = JsonSerializer.Deserialize<Root>(File.ReadAllText(path));

            //calculate durations
            foreach (var route in _metadata.Routes)
            {
                for (int i = 0; i < route.Path.Length; i++)
                {
                    if (i == 0)
                    {
                        route.Path[i].Duration = 0;
                        continue;
                    }

                    var distance = _metadata.BusStops[route.Path[i].BusStopIndex].GetDistance(_metadata.BusStops[route.Path[i - 1].BusStopIndex]);
                    route.Path[i].Duration = (int)(distance / Velocity);
                }
            }
        }

        public Root GetMetadata() => _metadata;
    }
}
