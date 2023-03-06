using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalR.Server.Controllers
{
    public class HubController : Controller
    {
        private readonly IHubContext<ChatHub> hubContext;

        public HubController(IHubContext<ChatHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SendMsgToAll(string name,string msg)
        {
            await hubContext.Clients.All.SendAsync("broadcastMessage", name, msg);
            return View("Index");
        }
    }
}