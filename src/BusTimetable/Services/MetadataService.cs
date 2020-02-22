using System.IO;
using System.Reflection;
using System.Text.Json;
using Models;

namespace BusTimetable.Services
{
    public class MetadataService : IMetadataService
    {
        private static readonly Root Metadata;

        static MetadataService()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "metadata.json");
            Metadata = JsonSerializer.Deserialize<Root>(File.ReadAllText(path));
        }

        public Root GetMetadata() => Metadata;
    }
}
