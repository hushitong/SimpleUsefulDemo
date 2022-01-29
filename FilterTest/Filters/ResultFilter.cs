using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace FilterTest.Filters
{
    public class ResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine($"****ResultFilter.OnResultExecuting, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine($"****ResultFilter.OnResultExecuted, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");
        }
    }

    public class ResultFilterAsync : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            //Before Action，相当于同步的OnResultExecuting
            await next();
            //After Action，相当于同步的OnResultExecuted
        }
    }
}
