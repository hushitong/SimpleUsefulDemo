using System;

namespace ASP.NETCoreMini.Core
{
    public interface IMiniWebHostBuilder
    {
        //用来注册服务器
        IMiniWebHostBuilder UseServer(IMiniServer server);
        //用来注册中间件
        IMiniWebHostBuilder Configure(Action<IMiniApplicationBuilder> configure);
        //用来创建WebHost
        IMiniWebHost Build();
    }
}
