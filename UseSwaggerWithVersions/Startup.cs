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

namespace UseSwaggerWithVersions
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

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                //遍历版本信息
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version, //版本号
                        Title = $"My API {version}", //标题
                        Description = $"My ASP.NET Core Web API {version}", //描述
                        //TermsOfService = new Uri("https://example.com/terms"), //服务条款
                        //Contact = new OpenApiContact
                        //{
                        //    Name = "Singo", //联系人
                        //    Email = string.Empty,  //邮箱
                        //    Url = new Uri("https://github.com/hushitong"),//网站
                        //},
                        //License = new OpenApiLicense
                        //{
                        //    Name = "Use under LICX", //协议
                        //    Url = new Uri("https://example.com/license"), //协议地址
                        //}
                    });
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;

                typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    //描述终结点的json文档
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);
                    //设置为none可折叠所有方法
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    //设置为-1可不显示models
                    //c.DefaultModelsExpandDepth(-1);
                });
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
