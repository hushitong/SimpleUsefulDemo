using FilterTest.Attributes;
using FilterTest.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FilterTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterTestController : ControllerBase
    {
        [HttpGet("TestException")]
        //[TypeFilter(typeof(ExceptionFilter))]
        //[ServiceFilter(typeof(ExceptionFilter))]
        public IActionResult TestException()
        {
            throw null;
            return Ok();
        }

        [HttpPost]
        [AddHeader]
        public void Get(Person person)
        {
            Console.WriteLine($"****WeatherForecastController.Get, Request Path: {HttpContext.Request.Path}");
        }

        public class Person
        {
            public Person()
            {
                Console.WriteLine($"****Model Binding, Person.Ctor.");
                //throw new Exception("Model Binding Exception");
            }

            public string Name { get; set; }

            public int Age { get; set; }
        }

        [HttpGet]
        [MyActionFilter(Order = -2)]
        [MyActionFilterWithDI(Order = -1)]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
