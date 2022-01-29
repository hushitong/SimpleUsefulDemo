using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace UseSwaggerWithVersions
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = nameof(ApiVersion.V1))]
    public class HealthController : Controller
    {
        private readonly IConfiguration _iConfiguration;

        public HealthController(IConfiguration configuration)
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
            return $"Health check OK! Now is {DateTime.Now.ToString()}!";
        }
    }
}
