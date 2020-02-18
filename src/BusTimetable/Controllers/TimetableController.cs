using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BusTimetable.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimetableController : ControllerBase
    {
        private readonly ILogger<TimetableController> _logger;

        public TimetableController(ILogger<TimetableController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
