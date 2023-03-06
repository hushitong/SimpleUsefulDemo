using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OAuth.Client
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
            
            services.AddAuthentication(config =>
            {
                config.DefaultSignInScheme = "MyCookieScheme";  //当我们登陆后，设置Cookie
                config.DefaultAuthenticateScheme = "MyCookieScheme";  //使用作为验证是否登陆
                config.DefaultChallengeScheme = "OurServer";  //使用来检查是否具有权限
            }).AddCookie("MyCookieScheme", options =>
            {
                //设置密钥存储位置，你也可以使用services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"C:\temp-keys2\"))
                //options.DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo(@"C:\temp-keys2\"));
                options.DataProtectionProvider = DataProtectionProvider.Create("MyDataProtection");
                options.Cookie.Name = "MyClientCookie";
            })
            .AddMyOAuth("OurServer");
            
            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            bool ShowServicesListOnConsole = true;
            //ShowServicesListOnConsole = Configuration.GetValue<bool>("ShowServicesListOnConsole");
            if (ShowServicesListOnConsole)
            {
                Console.WriteLine(GetDIShowList(services));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// 获取当前注册的所有服务，并打印每个服务对应的声明类型、实现类型和生命周期
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public string GetDIShowList(IServiceCollection services)
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
    }
}
