using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Client.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceProvider serviceProvider;

        public AuthController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        //不需要验证
        [HttpGet]
        public ActionResult GetWithoutAuth()
        {
            HttpContext.Response.Cookies.Append("web_nmae", "MyWeb");
            return Ok("GetWithoutAuth访问成功");
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetWithAuth()
        {
            //使用HttpContext.User.Claims可以获得当前用户Payload里的信息
            HttpContext.User.Claims.ToList().ForEach(x => Console.WriteLine(x));
            //var d = DecryptAuthCookie(HttpContext);
            return Ok("GetWithAuth访问成功");
        }

        //验证是否通过同时还得验证其payload中是否由符合SuperAdmin的Role
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult GetWithRoleAuth()
        {
            return Ok("GetWithRoleAuth访问成功");
        }

        [HttpGet]
        public async Task<ActionResult> SignIn()
        {
            var claims = new Claim[] {
                    new Claim(ClaimTypes.Name,"singo"),
                    new Claim(ClaimTypes.NameIdentifier,"4433"),
                    new Claim(ClaimTypes.Role,"system"),
                    new Claim(ClaimTypes.Role,"admin")
             };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var d1 = serviceProvider.GetRequiredService<IOptionsMonitor<CookieAuthenticationOptions>>();
            var d2 = serviceProvider.GetRequiredService<IOptionsMonitor<AuthenticationOptions>>();
            var d3 = serviceProvider.GetRequiredService<IConfigureOptions<CookieAuthenticationOptions>>();
            var d4 = serviceProvider.GetRequiredService<IPostConfigureOptions<CookieAuthenticationOptions>>();
            var claims2 = new Claim[] {
                    new Claim(ClaimTypes.Name,"singo2"),
                    new Claim(ClaimTypes.NameIdentifier,"44332"),
                    new Claim(ClaimTypes.Role,"system2"),
                    new Claim(ClaimTypes.Role,"admin2")
             };
            ClaimsIdentity claimsIdentity2 = new ClaimsIdentity(claims2, "cook");

            var claimsPrincipal = new ClaimsPrincipal(new List<ClaimsIdentity> { claimsIdentity2, claimsIdentity });
            await HttpContext.SignInAsync(claimsPrincipal);
            return Ok("SignIn成功");
        }

        //解密Cookie
        [HttpGet]
        public IActionResult DecryptCookie()
        {
            //获得加密了的Cookie值，Cookie名假如注册时候没指定的话为：.AspNetCore.Cookies
            string cookieValue = HttpContext.Request.Cookies["MyClientCookie"];

            //使用Create方法生成DataProtectionProvider对象，密钥存储位置值与在ConfigureServices注册时一致
            //var provider = DataProtectionProvider.Create(new DirectoryInfo(@"C:\temp-keys2\"));
            var provider = DataProtectionProvider.Create("MyDataProtection");

            //获得DataProtector对象，第二个参数的值你在注册Cookies时候设定的Scheme名，没有设定的话其默认值为：Cookies
            var dataProtector = provider.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", "MyCookieScheme", "v2");

            //解密Cookie值，变为普通文本
            //方法1：
            var unprotectedData = dataProtector.Unprotect(cookieValue);
            //方法2：
            //byte[] protectedBytes = Base64UrlTextEncoder.Decode(cookieValue);
            //byte[] plainBytes = dataProtector.Unprotect(protectedBytes);
            //string unprotectedData = Encoding.UTF8.GetString(plainBytes);
            //方法3：
            //UTF8Encoding specialUtf8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false);
            //string unprotectedData = specialUtf8Encoding.GetString(plainBytes);

            //或解密Cookie值，转换为AuthenticationTicket对象
            TicketDataFormat ticketDataFormat = new TicketDataFormat(dataProtector);
            AuthenticationTicket ticket = ticketDataFormat.Unprotect(cookieValue);

            string strPrincipal = string.Empty;
            foreach (var identity in ticket.Principal.Identities)
            {
                strPrincipal += $"AuthenticationType: {identity.AuthenticationType}\n";
                foreach (var claim in identity.Claims)
                {
                    strPrincipal += $"type:{claim.Type}, value:{claim.Value} \n";
                }
            }
            var strProperties = JsonConvert.SerializeObject(ticket.Properties.Items);

            return Ok(strPrincipal + "\n Properties: \n" + strProperties);
        }

        //public static AuthenticationTicket DecryptAuthCookie(HttpContext httpContext)
        //{
        //    // ONE - grab the CookieAuthenticationOptions instance
        //    var opt = httpContext.RequestServices
        //        .GetRequiredService<IDataProtectionProvider>();
        //}
    }
}