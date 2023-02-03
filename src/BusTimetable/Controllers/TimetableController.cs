using System;
using System.Threading.Tasks;
using BusTimetable.Grains;
using BusTimetable.Services;
using Microsoft.AspNetCore.Mvc;
using Models;
using Orleans;

namespace BusTimetable.Controllers;

[ApiController]
public class TimetableController : ControllerBase
{
    private readonly IMetadataService _metadata;
    private readonly ITimetableCache _cache;
    private readonly IClusterClient _clusterClient;

    public TimetableController(IMetadataService metadata,
        ITimetableCache cache,
        IClusterClient clusterClient)
    {
        _metadata = metadata;
        _cache = cache;
        _clusterClient = clusterClient;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return Ok($"Hi, I'm {nameof(BusTimetable)} backend, server name {Environment.MachineName}, OS {Environment.OSVersion}");
    }

    [HttpGet("metadata")]
    public IActionResult GetMetadata()
    {
        return Ok(_metadata.GetMetadata());
    }

    [HttpPost("{routeId}/location")]
    public async Task<IActionResult> UpdateLocation(string routeId, [FromBody]Location location)
    {
        var routeGrain = _clusterClient.GetGrain<IRoute>(routeId);
        await routeGrain.UpdateLocation(location);

        return Ok();
    }

    [HttpGet("{busStopId}/timetable")]
    public async Task<IActionResult> GetTimetable(string busStopId)
    {
        var busStopGrain = _clusterClient.GetGrain<IBusStop>(busStopId);
        var timetable = await busStopGrain.GetTimetable();

        return Ok(timetable);
    }

    [HttpGet("{busStopId}/cache/timetable")]
    public IActionResult GetTimeTableFromCache(string busStopId)
    {
        return Ok(_cache.GetTimetable(busStopId));
    }
}
