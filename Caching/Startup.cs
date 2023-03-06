using HolyGrailWarModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Text;

namespace TestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddResponseCaching(); //Use ResponseCaching
            services.AddMemoryCache();  //Use In-MemoryCache

            //Use Redis，需要你本地启动了Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost";  //使用本地Redis
                options.InstanceName = "SampleInstance_";  //添加前缀可以区别于其他应用来的数据
            });

            //Use UseNpgsql
            string strConn = @"Host=localhost;Database=HolyGrailWar;Username=postgres;Password=password";
            services.AddDbContext<HolyGrailWarDbContext>(options =>
                options.UseNpgsql(strConn));

            #region 获取当前注册的所有服务，并打印每个服务对应的声明类型、实现类型和生命周期
            var ShowServicesListOnConsole = false;  
            //var ShowServicesListOnConsole = Configuration.GetSection("ShowServicesListOnConsole").Value == "true";
            if (ShowServicesListOnConsole)
            {
                Console.WriteLine(GetServicesListShowString(services));
            }
            #endregion
        }

        /// <summary>
        /// 获取当前注册的所有服务，并打印每个服务对应的声明类型、实现类型和生命周期
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public string GetServicesListShowString(IServiceCollection services)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Services Count: {services.Count}");
            var provider = services.BuildServiceProvider();
            foreach (var service in services)
            {
                var serviceTypeName = GetName(service.ServiceType);
                var implementationType = service.ImplementationType
                        ?? service.ImplementationInstance?.GetType()
                        ?? service.ImplementationFactory?.Invoke(provider)?.GetType();
                if (implementationType != null)
                {
                    sb.AppendLine($"{service.Lifetime,-15} {serviceTypeName,-50}{GetName(implementationType)}");
                }
            }

            return sb.ToString();

            string GetName(Type type)
            {
                if (!type.IsGenericType)
                {
                    return type.Name;
                }
                var name = type.Name.Split('`')[0];
                var args = type.GetGenericArguments().Select(it => it.Name);
                return $"{name}<{string.Join(",", args)}>";
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();

            app.UseResponseCaching();  //Use ResponseCaching

            //列出了所有的配置项
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/dump-config", async ctx =>
                    {
                        var configInfo = (Configuration as IConfigurationRoot).GetDebugView();
                        await ctx.Response.WriteAsync(configInfo);
                    });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
