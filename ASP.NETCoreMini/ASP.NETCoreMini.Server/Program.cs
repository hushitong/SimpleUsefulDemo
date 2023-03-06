using ASP.NETCoreMini.Core;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NETCoreMini.Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await new MiniWbeHostBuilder()
                //.UseHttpListener()
                .UseMyHttpListener()  //����ʹ�õ�IServer�Ḳ��ǰ�����õģ����ֻ������IServer��Ч
                .Configure(conifg => conifg
                    .Use(One)
                    .Use(Two)
                    .Use(Last))
                //.Configure(conf => conf.Use(Three).Use(Last))
                .Build()
                .StartAsync();
        }

        

        private static RequestDelegate One(RequestDelegate next)
        {
            return async context =>
            {
                await context.Response.WriteAsync("One In => ");
                await next(context);
                await context.Response.WriteAsync(" => One Out");
            };
        }
        private static RequestDelegate Two(RequestDelegate next)
        {
            return async context =>
            {
                await context.Response.WriteAsync("Two In => ");
                await next(context);
                await context.Response.WriteAsync(" => Two Out");
            };
        }
        private static RequestDelegate Three(RequestDelegate next)
        {
            return async context =>
            {
                await context.Response.WriteAsync("Three In => ");
                await next(context);
                await context.Response.WriteAsync(" => Three Out");
            };
        }
        private static RequestDelegate Last(RequestDelegate next)
        {
            return async context =>
            {
                await context.Response.WriteAsync("Last");
            };
        }
    }
}