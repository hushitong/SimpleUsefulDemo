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

            //ע��Swagger������������һ���Ͷ��Swagger �ĵ�
            services.AddSwaggerGen(c =>
            {
                //�����汾��Ϣ
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version, //�汾��
                        Title = $"My API {version}", //����
                        Description = $"My ASP.NET Core Web API {version}", //����
                        //TermsOfService = new Uri("https://example.com/terms"), //��������
                        //Contact = new OpenApiContact
                        //{
                        //    Name = "Singo", //��ϵ��
                        //    Email = string.Empty,  //����
                        //    Url = new Uri("https://github.com/hushitong"),//��վ
                        //},
                        //License = new OpenApiLicense
                        //{
                        //    Name = "Use under LICX", //Э��
                        //    Url = new Uri("https://example.com/license"), //Э���ַ
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

            //�����м����������Swagger��ΪJSON�ս��
            app.UseSwagger();
            //�����м�������swagger-ui��ָ��Swagger JSON�ս��
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;

                typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    //�����ս���json�ĵ�
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);
                    //����Ϊnone���۵����з���
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    //����Ϊ-1�ɲ���ʾmodels
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
