using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestApi.Controllers.Cache
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private IDistributedCache _cache;
        public RedisController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet("Get")]
        public string Get()
        {
            //获取
            var obj = _cache.Get("id1");
            return obj == null ? "Err：Not Found!" : Encoding.Default.GetString(obj);
        }

        [HttpGet("Set")]
        public string Set()
        {
            var obj = _cache.Get("id1");
            if (obj == null)
            {
                int absoluteExpirationRelativeToNowSec = 100;
                int slidingExpirationSec = 50;

                //添加，注意：添加Redis后的类型是hash
                //默认没有过期时间
                //_cache.Set("id1", Encoding.Default.GetBytes("永不过期，缓存时间是：{DateTime.Now.ToLongTimeString()}"), new DistributedCacheEntryOptions { });
                //同样可以设置绝对过期时间与滑动过期时间
                _cache.Set(
                    "id1",
                    Encoding.Default.GetBytes($"缓存时间是：{DateTime.Now.ToLongTimeString()}，过期时间是：{DateTime.Now.AddSeconds(absoluteExpirationRelativeToNowSec).ToLongTimeString()}"),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(absoluteExpirationRelativeToNowSec),
                        SlidingExpiration = TimeSpan.FromSeconds(slidingExpirationSec)
                    }
                );
            }

            obj = _cache.Get("id1");
            return obj == null ? "Err：Not Found!" : Encoding.Default.GetString(obj);
        }

        [HttpGet("Del")]
        public void Del()
        {
            //移除
            _cache.Remove("id1");
        }

        [HttpGet("Refresh")]
        public string Refresh()
        {
            //刷新，Get命令同样会刷新滑动过期时间
            _cache.Refresh("id1");

            var obj = _cache.Get("id1");
            return obj == null ? "Err：Not Found!" : Encoding.Default.GetString(obj);
        }
    }
}
