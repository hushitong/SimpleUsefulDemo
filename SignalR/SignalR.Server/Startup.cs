using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server
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
            services.AddControllersWithViews();

            //ע��SignalRʵʱͨѶ��Ĭ����json����
            services.AddSignalR(options =>
            {
                //����˷��������������ͻ��˼���Ƿ����ߵļ��
                //Ĭ��15�룬�ĳ�2���ӣ���ҳ���������connection.serverTimeoutInMilliseconds = 24e4;��4����
                options.KeepAliveInterval = TimeSpan.FromMinutes(2);
                //����˽��տͻ��ˡ�������������������ʱ����Ϊ�ͻ��������߻������ϵ�����
                //Ĭ��30�룬�ĳ�4���ӣ���ҳ���������connection.keepAliveIntervalInMilliseconds = 12e4;��2����
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(4);
            });

            services.AddCors(option =>
                option.AddDefaultPolicy(policy => 
                    policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:8888", "https://localhost:8889")
                )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //���WebSocket֧�֣�SignalR����ʹ��WebSocket����
            app.UseWebSockets();
            //���ÿ�������֧��
            app.UseCors();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chathub");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
