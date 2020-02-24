using System.IO;
using System.Reflection;
using System.Text.Json;
using Models;

namespace BusTimetable.Services
{
    public class MetadataService : IMetadataService
    {
        private static readonly Root Metadata; 
        private const decimal Velocity = 0.1m; //pixels in milliseconds

        static MetadataService()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "metadata.json");
            Metadata = JsonSerializer.Deserialize<Root>(File.ReadAllText(path));

            //calculate durations
            foreach (var route in Metadata.Routes)
            {
                for (int i = 0; i < route.Path.Length; i++)
                {
                    if (i == 0)
                    {
                        route.Path[i].Duration = 0;
                        continue;
                    }

                    var distance = Metadata.BusStops[route.Path[i].BusStopIndex].GetDistance(Metadata.BusStops[route.Path[i - 1].BusStopIndex]);
                    route.Path[i].Duration = (int)(distance / Velocity);
                }
            }
        }

        public Root GetMetadata() => Metadata;
    }
}
