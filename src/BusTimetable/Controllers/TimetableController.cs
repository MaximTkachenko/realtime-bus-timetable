using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace BusTimetable.Controllers
{
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private readonly ILogger<TimetableController> _logger;
        private static readonly Root Metadata;

        static TimetableController()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "metadata.json");
            Metadata = JsonSerializer.Deserialize<Root>(System.IO.File.ReadAllText(path));
        }

        public TimetableController(ILogger<TimetableController> logger)
        {
            _logger = logger;
        }

        [HttpGet("metadata")]
        public IActionResult GetMetadata()
        {
            return Ok(Metadata);
        }

        [HttpPost("location")]
        public IActionResult Location(string routeId, int x, int y)
        {
            //todo return bus stops and routes
            return Ok();
        }
    }
}
