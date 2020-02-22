using BusTimetable.Models;
using BusTimetable.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BusTimetable.Controllers
{
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private readonly ILogger<TimetableController> _logger;
        private readonly IMetadataService _metadata;

        public TimetableController(ILogger<TimetableController> logger,
            IMetadataService metadata)
        {
            _logger = logger;
            _metadata = metadata;
        }

        [HttpGet("metadata")]
        public IActionResult GetMetadata()
        {
            return Ok(_metadata.GetMetadata());
        }

        [HttpPost("{routeId}/location")]
        public IActionResult Location(string routeId, [FromBody]Location location)
        {
            _logger.LogInformation("{routeId}: {x}, {y}", routeId, location.X, location.Y);

            //todo call grain

            return Ok();
        }
    }
}
