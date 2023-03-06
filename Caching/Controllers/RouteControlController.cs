using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteControlController : ControllerBase
    {
        /// <summary>
        /// 使用HttpContext.Request.RouteValues获得请求地址匹配的值
        /// </summary>
        /// <returns></returns>
        [HttpGet("/school/{school}/class/{class}/position/{row}.{col}")]
        public string GetRouteValues()
        {
            string str = string.Empty;

            var values = HttpContext.Request.RouteValues;
            foreach (var value in values)
            {
                str += $"{value.Key} = {value.Value} \n";
            }
            return str;
        }

        /// <summary>
        /// 使用*表示后面的内容都归position管
        /// 如地址：/school/gduss/no200109897/room404/bed04，那position获得的部分就是no200109897/room404/bed04
        /// </summary>
        /// <returns></returns>
        [HttpGet("/school/{school}/{*position}")]
        public string GetRouteValues2()
        {
            string str = string.Empty;

            var values = HttpContext.Request.RouteValues;
            foreach (var value in values)
            {
                str += $"{value.Key} = {value.Value} \n";
            }
            return str;
        }
    }
}
