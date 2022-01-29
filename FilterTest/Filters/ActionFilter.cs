using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace FilterTest.Filters
{
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"****ActionFilter.OnActionExecuting, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");
            //throw new Exception("ActionFilter.OnActionExecuting Exception");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"****ActionFilter.OnActionExecuted, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");
        }
    }

    public class ActionFilter2 : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"****ActionFilter2.OnActionExecuting, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");
            //throw new Exception("ActionFilter.OnActionExecuting Exception");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"****ActionFilter2.OnActionExecuted, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");
        }
    }

    public class ActionFilterWithDI : IActionFilter, IOrderedFilter
    {
        private readonly IConfiguration configuration;

        public int Order { get; set; }

        public ActionFilterWithDI(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"****ActionFilterWithDI.OnActionExecuting, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");
            //throw new Exception("ActionFilter.OnActionExecuting Exception");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"****ActionFilterWithDI.OnActionExecuted, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");
        }
    }

    public class ActionFilterAsync : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Before Action，相当于同步的OnActionExecuting
            await next();
            //After Action，相当于同步的OnActionExecuted
        }
    }
}
