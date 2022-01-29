using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace FilterTest.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Console.WriteLine($"========================Begin Filter Pipeline========================");
            Console.WriteLine($"****AuthorizationFilter.OnAuthorization, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");

            //判断是否能被匿名调用
            var action = context.ActionDescriptor as ControllerActionDescriptor;
            var allowAnonymousAttr = action.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>();
            bool isAllowAnonymous = allowAnonymousAttr == null ? false : true;

            //能被匿名调用，则继续执行；
            //不能被匿名调用，则验证是否授权访问，如果授权未通过，则抛出“未授权访问”信息
            try
            {
                if (isAllowAnonymous == false)
                {
                    //验证是否授权访问，如果授权未通过，则抛出“未授权访问”信息，流程不在往下走
                }

            }
            catch (Exception ex)
            {
                //对异常进行处理，需要注意抛出的异常不能被Exception Filter所捕获，因此不要想着直接throw出去
                context.Result = new ObjectResult(new { rtnCode = 500, msg = "鉴权服务发生错误，请稍后重试或联系管理人员" }); ;
            }
        }
    }

    public class AuthorizationFilterAsync : IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
