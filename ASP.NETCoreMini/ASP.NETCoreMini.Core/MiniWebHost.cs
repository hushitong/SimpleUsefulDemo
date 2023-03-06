using System.Threading.Tasks;

namespace ASP.NETCoreMini.Core
{
    public class MiniWebHost : IMiniWebHost
    {
        private readonly IMiniServer _server;
        private readonly RequestDelegate _handler;
        public MiniWebHost(IMiniServer server, RequestDelegate handler)
        {
            _server = server;
            _handler = handler;
        }
        public Task StartAsync() => _server.StartAsync(_handler);
    }
}
