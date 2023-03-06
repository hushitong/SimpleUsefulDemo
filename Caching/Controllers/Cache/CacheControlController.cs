using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestApi.Controllers.Cache
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheControlController : ControllerBase
    {
        // GET: api/<CacheControlController>
        [HttpGet]
        [ResponseCache(Duration = 1)]
        public System.DateTime Get()
        {
            var d = HttpContext.Items;
            return System.DateTime.Now;
        }
    }
}
