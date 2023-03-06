using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace JWT.DemoApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JWTTestController : ControllerBase
    {
        //不需要验证JWT
        [HttpGet]
        public ActionResult GetWithoutAuth()
        {
            return Ok("访问成功");
        }

        //只验证JWT是否通过
        [HttpGet]
        [Authorize]
        public ActionResult GetWithAuth()
        {
            //使用HttpContext.User.Claims可以获得当前用户Payload里的信息
            HttpContext.User.Claims.ToList().ForEach(x => Console.WriteLine(x));
            return Ok("访问成功");
        }

        //验证JWT是否通过同时还得验证其payload中是否由符合SuperAdmin的Role
        [HttpGet]
        [Authorize(Roles ="SuperAdmin")]
        public ActionResult GetWithRoleAuth()
        {
            return Ok("访问成功");
        }

        [HttpGet]
        public ActionResult SetCookiesToken(string access_token)
        {
            HttpContext.Response.Cookies.Append("access_token", access_token);
            return Ok("设置Cookies成功");
        }
    }
}