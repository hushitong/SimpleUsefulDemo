using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace FilterTest.Filters
{
    public class ResourceFilter : IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine($"****ResourceFilter.OnResourceExecuting, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");
            //throw new Exception("ResourceFilter.OnResourceExecuting Exception");
            //判断是否由以该Request.Path为key的缓存，有就获得缓存内的value，然后构造response再返回
            var cacheKey = context.HttpContext.Request.Path;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine($"****ResourceFilter.OnResourceExecuted, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");
            //throw new Exception("ResourceFilter.OnResourceExecuted Exception");

            //判断是否由以该Request.Path为key的缓存，没有就保存到缓存，再返回
            var cacheKey = context.HttpContext.Request.Path;
        }
    }

    public class ResourceFilterAsync : IAsyncResourceFilter
    {
        public Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
