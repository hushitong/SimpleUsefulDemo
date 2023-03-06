using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Server.Controllers
{
    public class OAuthController : Controller
    {
        readonly IConfiguration configuration;

        public OAuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// 用户登陆，并提示Client传的scope里索取了用户什么信息
        /// </summary>
        /// <param name="response_type"></param>
        /// <param name="client_id"></param>
        /// <param name="redirect_uri"></param>
        /// <param name="scope"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Authorize(string response_type, string client_id, string redirect_uri, string scope, string state)
        {
            //验证client_id与是否合法
            if (client_id != "APP_001")
                return BadRequest("非法client_id，请求拒绝！");

            var query = new QueryBuilder();
            query.Add(nameof(redirect_uri), redirect_uri);
            query.Add(nameof(scope), scope);
            query.Add(nameof(state), state);

            ViewData["scope"] = scope;
            return View(model: query.ToString());
        }

        /// <summary>
        /// 生成Authorizaion Code并返回给Client
        /// </summary>
        /// <param name="redirect_uri"></param>
        /// <param name="scope"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Authorize(string redirect_uri, string scope, string state)
        {
            //验证用户名与密码

            var query = new QueryBuilder();
            query.Add(nameof(redirect_uri), redirect_uri);
            query.Add(nameof(scope), scope);
            query.Add("code", "TheAuthorizationCode");
            query.Add(nameof(state), state);

            return Redirect(redirect_uri + query.ToString());
        }

        /// <summary>
        /// 用于Client使用Authorizaion Code申请Access Token
        /// </summary>
        /// <param name="client_id"></param>
        /// <param name="redirect_uri">获得Access Token后要调用Client接口</param>
        /// <param name="code">Client前面获得的Authorizaion Code</param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetToken(string client_id, string redirect_uri, string code, string state)
        {
            //To Do：校验、认证等代码

            //var tokenStr = TokenHelper.GetToken("Bearer", configuration.GetSection("Jwt"));
            var tokenStr = TokenHelper.GetToken("diy");
            var respBytes = Encoding.UTF8.GetBytes(tokenStr);
            await Response.Body.WriteAsync(respBytes, 0, respBytes.Length);

            return Redirect(redirect_uri);
        }


        /// <summary>
        /// 用于Client使用Access Token获得更多
        /// </summary>
        /// <param name="client_id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserInfo(string client_id, string state)
        {
            var respObject = new
            {
                name = "singo",
                email = "hushitong@63.com",
                age = 34,
                role = "admin"
            };

            var reqBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(respObject));
            await Response.Body.WriteAsync(reqBytes, 0, reqBytes.Length);

            return Ok();
        }
    }
}
