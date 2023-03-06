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
                config.DefaultSignInScheme = "MyCookieScheme";  //�����ǵ�½������Cookie
                config.DefaultAuthenticateScheme = "MyCookieScheme";  //ʹ����Ϊ��֤�Ƿ��½
                config.DefaultChallengeScheme = "OurServer";  //ʹ��������Ƿ����Ȩ��
            }).AddCookie("MyCookieScheme", options =>
            {
                //������Կ�洢λ�ã���Ҳ����ʹ��services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"C:\temp-keys2\"))
                //options.DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo(@"C:\temp-keys2\"));
                options.DataProtectionProvider = DataProtectionProvider.Create("MyDataProtection");
                options.Cookie.Name = "MyClientCookie";
            })
            .AddMyOAuth("OurServer");
            
            //ע��Swagger������������һ���Ͷ��Swagger �ĵ�
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

            //�����м����������Swagger��ΪJSON�ս��
            app.UseSwagger();
            //�����м�������swagger-ui��ָ��Swagger JSON�ս��
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
        /// ��ȡ��ǰע������з��񣬲���ӡÿ�������Ӧ���������͡�ʵ�����ͺ���������
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
