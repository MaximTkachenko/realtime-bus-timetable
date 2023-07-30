using System.IO;
using System.Reflection;
using System.Text.Json;
using Models;

namespace BusTimetable.Services;

public class MetadataV2
{
    private readonly Meta _metadata; 

    public MetadataV2()
    {
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "metadataV2.json");
        _metadata = JsonSerializer.Deserialize<Meta>(File.ReadAllText(path));
    }

    public Meta GetMetadata() => _metadata;
}   