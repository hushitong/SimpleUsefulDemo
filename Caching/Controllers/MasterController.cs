using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HolyGrailWarModel;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly HolyGrailWarDbContext holyGrailWarDbContext;

        public MasterController(HolyGrailWarDbContext holyGrailWarDbContext)
        {
            this.holyGrailWarDbContext = holyGrailWarDbContext;
        }

        [HttpGet("/GetMasters")]
        public IEnumerable<Master> GetMasters()
        {
            return holyGrailWarDbContext.Masters;
        }

        [HttpPut("/AddMasters")]
        public IActionResult AddMasters()
        {
            var master1 = holyGrailWarDbContext.Masters.Add(new Master { Name = $"Shirou Emiya" });
            var master2 = holyGrailWarDbContext.Masters.Add(new Master { Name = $"Osaka Rin" });
            var master3 = holyGrailWarDbContext.Masters.Add(new Master { Name = $"Illyasviel von Einzbern" });
            holyGrailWarDbContext.SaveChanges();

            string str = string.Empty;
            var masters = holyGrailWarDbContext.Masters;
            foreach (var master in masters)
            {
                str += $"ID:{master.MasterId,4}, Name:{master.Name} \n";
            }
            return Ok($"Add masters OK, Now has below masters: \n{str}");
        }
    }
}
