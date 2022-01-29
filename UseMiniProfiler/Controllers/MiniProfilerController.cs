using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;
using System.Collections.Generic;

namespace UseMiniProfiler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiniProfilerController : ControllerBase
    {
        [HttpGet("GetMiniProfilerScript")]
        public IActionResult GetMiniProfilerScript()
        {
            var html = MiniProfiler.Current.RenderIncludes(HttpContext);
            return Ok(html.Value);
        }
    }
}
