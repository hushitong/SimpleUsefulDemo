using System.Threading.Tasks;

namespace ASP.NETCoreMini.Core
{
    public interface IMiniServer
    {
        Task StartAsync(RequestDelegate handler);
    }
}
