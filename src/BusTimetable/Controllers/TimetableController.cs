using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BusTimetable.Controllers
{
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private readonly ILogger<TimetableController> _logger;

        public TimetableController(ILogger<TimetableController> logger)
        {
            _logger = logger;
        }

        [HttpGet("metadata")]
        public IActionResult Metadata()
        {
            //todo return bus stops and routes
            return Ok();
        }

        [HttpPost("location")]
        public IActionResult Location(string routeId, int x, int y)
        {
            //todo return bus stops and routes
            return Ok();
        }
    }
}
