using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using FilterTest.Filters;

namespace FilterTest
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
            services.AddControllers(config =>
            {
                config.Filters.Add<AuthorizationFilter>();
                config.Filters.Add<ResourceFilter>();
                config.Filters.Add<ExceptionFilter>();
                config.Filters.Add<ActionFilter>();
                config.Filters.Add<ResultFilter>();
            });

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            //注册MiniProfiler
            services.AddMiniProfiler(options =>
              //配置MiniProfiler的路由基础路径，按照当前配置，你可以使用"/profiler/results"来访问分析报告
              options.RouteBasePath = "/profiler"
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //启用中间件服务生成Swagger作为JSON终结点
                app.UseSwagger();
                //启用中间件服务对swagger-ui，指定Swagger JSON终结点
                app.UseSwaggerUI(c =>
                {
                    string projectName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                    c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream($"{projectName}.index.html");

                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });

                //启用MiniProfiler服务
                app.UseMiniProfiler();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/dump-config", async ctx =>
                {
                    var configInfo = (Configuration as IConfigurationRoot).GetDebugView();
                    await ctx.Response.WriteAsync(configInfo);
                });
            });

            //app.Map("/TM", HostBuilder => {
            //    HostBuilder.Use(async (context, next) =>
            //    {
            //        await context.Response.WriteAsync("1 in /r/n");
            //        await next();
            //        await context.Response.WriteAsync("1 out <br/>");
            //    });
            //    HostBuilder.Use(async (context, next) =>
            //    {
            //        await context.Response.WriteAsync("2 in <br/>");
            //        await next();
            //        await context.Response.WriteAsync("2 out <br/>");
            //    });
            //    HostBuilder.Run(async context => {
            //        await context.Response.WriteAsync("Ruaaaaaaaa");
            //    });
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
