using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ASP.NETCoreMini.Core
{
    public class HttpListenerServer : IMiniServer
    {
        private readonly HttpListener _httpListener;
        private readonly string[] _urls;

        public HttpListenerServer(params string[] urls)
        {
            //构造一个HttpListenerServer对象
            _httpListener = new HttpListener();
            //没有提供监听地址就默认监听”localhost:5000“
            _urls = urls.Any() ? urls : new string[] { "http://localhost:5000/" };
        }

        public async Task StartAsync(RequestDelegate handler)
        {
            Array.ForEach(_urls, url => _httpListener.Prefixes.Add(url));
            _httpListener.Start();
            while (true)
            {
                //通过GetContextAsync方法实现了针对请求的监听和接收
                var listenerContext = await _httpListener.GetContextAsync();
                var feature = new HttpListenerFeature(listenerContext);
                var features = new FeatureCollection()
                    .Set<IHttpRequestFeature>(feature)
                    .Set<IHttpResponseFeature>(feature);
                var httpContext = new MiniHttpContext(features);
                Console.WriteLine(httpContext.Request.Url);
                await handler(httpContext);
                listenerContext.Response.Close();
            }
        }
    }

    public static partial class Extensions
    {
        public static IMiniWebHostBuilder UseHttpListener(this IMiniWebHostBuilder builder, params string[] urls)
         => builder.UseServer(new HttpListenerServer(urls));
    }
}
