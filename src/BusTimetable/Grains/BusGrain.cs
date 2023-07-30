using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BusTimetable.Controllers;
using BusTimetable.Services;
using Models;
using Orleans;

namespace BusTimetable.Grains;

public interface IBus : IGrainWithStringKey
{
    Task UpdateLocationAsync(GeoLocation location);
}

public class BusGrain: Grain, IBus
{
    private GeoLocation _lastReportedLocation;
    private Meta.Bus _busDetails;
    
    private readonly MetadataV2 _metadata;

    public BusGrain(MetadataV2 metadata)
    {
        _metadata = metadata;
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _busDetails = _metadata.GetMetadata().Buses[this.GetPrimaryKeyString()];
        
        return base.OnActivateAsync(cancellationToken);
    }

    public async Task UpdateLocationAsync(GeoLocation location)
    {
        _lastReportedLocation = location;
        var serializedLocation = new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(_lastReportedLocation)));
        await TimeTableV2Controller.Ws.SendAsync(serializedLocation, WebSocketMessageType.Text, true, CancellationToken.None);
    }
}