using FilterTest.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace FilterTest.Attributes
{
    public class MyActionFilterAttribute : Attribute, IActionFilter, IOrderedFilter
    {
        public int Order { get; set; }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"****MyActionFilterAttribute.OnActionExecuting, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"****MyActionFilterAttribute.OnActionExecuted, Request Path: {context.HttpContext.Request.Path}, Time: {DateTime.Now.ToString("hh:mm:ss ffff")}");
        }
    }

    public class MyActionFilterWithDIAttribute : TypeFilterAttribute
    {
        public MyActionFilterWithDIAttribute(): base(typeof(ActionFilterWithDI))
        {
        }
    }
}
