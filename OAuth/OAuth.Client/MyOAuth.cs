using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OAuth.Client
{
    public static class MyOAuth
    {
        public static AuthenticationBuilder AddMyOAuth(this AuthenticationBuilder builder, string authenticationScheme)
        {
            return builder.AddOAuth(authenticationScheme, options =>
            {
                options.ClientId = "APP_001";  //OAuth Server给应用，请求会带上，OAuth Server会验证其合法性
                options.ClientSecret = "My_Client_secret";
                options.CallbackPath = "/oauth/callback"; //获得Code后的回调函数，这地址由app.UseAuthentication()提供了，不需要自己写
                options.AuthorizationEndpoint = "https://localhost:44361/OAuth/Authorize";  //认证时，重定向到的地址（OAuth Server的
                options.TokenEndpoint = "https://localhost:44361/OAuth/GetToken";  //使用Code交换Token的地址（OAuth Server的),注意该地址需要能接收HttpPost请求
                options.UserInformationEndpoint = "https://localhost:44361/OAuth/GetUserInfo"; //获取认证成功后人员信息

                //Scope名由OAuth Server确定，代表不同的资源，应用可以申请，不过OAuth Server会审核是否给你，可以申请多个Scope，变成QueryString参数时多个Scope名会用空格隔开
                options.Scope.Add("BasicInfo"); //需要的用户基础信息，如：用户名、Email等
                options.Scope.Add("MoreInfo");  //需要的用户更多信息，如：电话号码、住址等

                options.SaveTokens = true; //是否向客户端的cookie中写入access_token、refresh_token、token_type

                //当然这里如果代码很多，可以直接去重写OAuthEvents这个事件
                options.Events = new OAuthEvents()
                {
                    OnCreatingTicket = async context =>
                    {
                        if (context.TokenType != "Bearer" && context.TokenType != "Jwt") //假如TokenType不是JWT，那还需要再去OAuth Server请求资源
                        {
                            //这里和主要是将用户信息写入Claim中，具体写入哪些到Claim中主要是下面options.ClaimActions去配置
                            //如果是options.ClaimActions.MapAll()就是将所有的用户信息写入Claim中
                            //如果是自定义加入就用options.ClaimActions.MapJsonKey()去指定
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();
                            var res = await response.Content.ReadAsStringAsync();
                            using (var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                            {
                                context.RunClaimActions(user.RootElement);
                            }
                        }
                        else  //假如TokenType是JWT，由JWT的Payload中获得需要的基础资源
                        {
                            var accessToken = context.AccessToken;

                            var base64Payload = accessToken.Split('.')[1];
                            //对字符串进行处理，去除不合法的字符
                            base64Payload = base64Payload.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
                            if (base64Payload.Length % 4 > 0)
                            {
                                base64Payload = base64Payload.PadRight(base64Payload.Length + 4 - base64Payload.Length % 4, '=');
                            }

                            var bytesPayload = Convert.FromBase64String(base64Payload);
                            var jsonPayload = Encoding.UTF8.GetString(bytesPayload);
                            using (var user = JsonDocument.Parse(jsonPayload))
                            {
                                context.RunClaimActions(user.RootElement);
                            }
                        }
                    },
                    OnTicketReceived = ticketcontext => //接受到票据的时候触发
                    {
                        return Task.CompletedTask;
                    }
                };

                //自定义获得什么信息写入Claim中，通过对比key
                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, ClaimTypes.NameIdentifier);
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, ClaimTypes.Name);
                options.ClaimActions.MapJsonKey(ClaimTypes.Role, ClaimTypes.Role);
                options.ClaimActions.MapJsonKey(ClaimValueTypes.Email, ClaimValueTypes.Email);
                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                options.ClaimActions.MapJsonKey(ClaimTypes.Role, "role");
                options.ClaimActions.MapJsonKey(ClaimValueTypes.Email, "email");
                options.ClaimActions.MapJsonKey("age", "age");
            });
        }
    }
}
