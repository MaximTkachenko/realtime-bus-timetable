using System.Net.WebSockets;
using System.Threading.Tasks;
using BusTimetable.Grains;
using BusTimetable.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Orleans;

namespace BusTimetable.Controllers;

[ApiController, Route("v2")]
public class TimeTableV2Controller : ControllerBase
{
    public static WebSocket Ws;
    
    private readonly IClusterClient _clusterClient;
    private readonly MetadataV2 _metadata;

    public TimeTableV2Controller(MetadataV2 metadata,
        IClusterClient clusterClient)
    {
        _clusterClient = clusterClient;
        _metadata = metadata;
    }

    [HttpPost("buses/{bus}/location")]
    public async Task<IActionResult> UpdateBusLocation(string bus, [FromBody]GeoLocation location)
    {
        var routeGrain = _clusterClient.GetGrain<IBus>(bus);
        await routeGrain.UpdateLocationAsync(location);

        return Ok();
    }

    [HttpGet("buses/location/ws")]
    public async Task GetBusLocations()
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
        
        Ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
        await new TaskCompletionSource().Task;
        Ws.Dispose();
    }

    [HttpGet("metadata")]
    public IActionResult GetMetadata()
    {
        return Ok(_metadata.GetMetadata());
    }
}