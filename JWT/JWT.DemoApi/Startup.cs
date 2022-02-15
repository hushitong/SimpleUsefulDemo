using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT.DemoApi
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                #region ���JWT֧��
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "���¿�����������ͷ����Ҫ���Jwt��ȨToken��Bearer Token",
                    Name = "Authorization", //������key��������ʱ������ϣ�Ĭ��ʹ��Authorization��
                    In = ParameterLocation.Header, //������ͷ���JWT Token
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                #endregion
            });

            //���ʹ�õ���Կ
            var jwtConfig = Configuration.GetSection("Jwt");
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.GetValue<string>("Secret")));
            //��֤����
            services.AddAuthentication("Bearer").AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,//�Ƿ���֤ǩ��,����֤�Ļ����Դ۸����ݣ�����ȫ
                    IssuerSigningKey = signingKey,//ʹ�õ���Կ

                    ValidateIssuer = true,//�Ƿ���֤ǩ���ߣ�������֤�غ��е�Iss�Ƿ��ӦValidIssuer����
                    ValidIssuer = jwtConfig.GetValue<string>("Iss"),//ǩ����
                    ValidateAudience = true,//�Ƿ���֤ʹ���ߣ�������֤�غ��е�Aud�Ƿ��ӦValidAudience����
                    ValidAudience = jwtConfig.GetValue<string>("Aud"),//ʹ����
                    ValidateLifetime = true,//�Ƿ���֤����ʱ�䣬�����˾;ܾ�����

                    ClockSkew = TimeSpan.FromSeconds(30),//����ǻ������ʱ�䣬Ҳ����˵����ʹ���������˹���ʱ�䣬����ҲҪ���ǽ�ȥ������ʱ��+���壬Ĭ�Ϻ�����7���ӣ������ֱ������Ϊ0
                    RequireExpirationTime = true, //�Ƿ�Ҫ��Token��Claims�б������Expires
                };
                //o.Events = new JwtBearerEvents()
                //{
                //    //���¼������յ���������֤JWTǰ����
                //    OnMessageReceived = context =>
                //    {
                //        //��ȡ��Cookies�е�access_token��������֤ʹ�ø�token�������ڿ���̨���
                //        context.Token = context.Request.Cookies["access_token"];
                //        Console.WriteLine(context.Token);
                //        return Task.CompletedTask;
                //    }
                //};
            });
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
    }
}
