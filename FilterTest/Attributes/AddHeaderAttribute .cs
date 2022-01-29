using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace FilterTest.Attributes
{
    public class AddHeaderAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine($"****AddHeaderAttribute.OnResultExecuting, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");
            context.HttpContext.Response.Headers.Add("Author", "Singo");
            //base.OnResultExecuting(context);
        }
    }
}
