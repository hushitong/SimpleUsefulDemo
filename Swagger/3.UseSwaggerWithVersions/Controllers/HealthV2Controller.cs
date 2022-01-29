using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace UseSwaggerWithVersions
{
    [Route("V2/api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = nameof(ApiVersion.V2))]
    public class HealthV2Controller : Controller
    {
        private readonly IConfiguration _iConfiguration;

        public HealthV2Controller(IConfiguration configuration)
        {
            _iConfiguration = configuration;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            Console.WriteLine($"Health check OK! Now is {DateTime.Now.ToString()}");
            return Ok();
        }

        [HttpGet]
        [Route("Check")]
        public string Check()
        {
            return $"Health check OK! Now is {DateTime.Now.ToString()}! Version V2";
        }
    }
}
