using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestApi.Controllers.Cache
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseCacheController : ControllerBase
    {
        // GET: api/<ResponseCacheController>
        [HttpGet]
        [ResponseCache(Duration = 5)]
        public System.DateTime Get()
        {
            return System.DateTime.Now;
        }
    }
}
