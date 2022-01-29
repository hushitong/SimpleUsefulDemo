using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;

namespace FilterTest.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IConfiguration configuration;

        public ExceptionFilter(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnException(ExceptionContext context)
        {
            Console.WriteLine($"****ExceptionFilter.OnException, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");

            ObjectResult result;
            var isShowDevExceptionMsg = "false";
            //isShowDevExceptionMsg = configuration.GetSection("IsShowDevExceptionMsg").Value;  //由配置得到
            if (!string.IsNullOrEmpty(isShowDevExceptionMsg) && isShowDevExceptionMsg.ToLower() == "true")
            {
                result = new ObjectResult(new { rtnCode = 500, msg = "服务发生错误，请稍后重试或联系管理人员", devMsg = context.Exception.Message });
            }
            else
            {
                result = new ObjectResult(new { rtnCode = 500, msg = "服务发生错误，请稍后重试或联系管理人员" });
            }
            result.StatusCode = 500;
            context.Result = result;
            context.ExceptionHandled = true;  //标记异常已被处理，异常不会再抛出，后续的Exception Filter不会再触发
        }
    }
}
