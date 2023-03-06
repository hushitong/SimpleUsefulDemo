using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestApi.Controllers.Cache
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemoryCacheController : ControllerBase
    {
        private IMemoryCache _cache;
        public MemoryCacheController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        [HttpGet]
        public string Get(int id)
        {
            string strName = "未知";

            //方法一、使用TryGetValue与Set
            //if (!_cache.TryGetValue("Name", out strName))
            //{
            //    strName = GetNmae(id);
            //    _cache.Set("Name", strName);
            //}
            //方法二、使用GetOrCreate
            //strName = _cache.GetOrCreate<string>("Name", (e) => { return GetNmae(id); });

            #region  过期时间策略
            ////不会过期
            //strName = _cache.GetOrCreate<string>("Name", (e) => { return GetNmae(id); });

            ////5秒后过期
            //strName = _cache.GetOrCreate<string>("Name", (e) => {
            //    e.AbsoluteExpirationRelativeToNow = System.TimeSpan.FromSeconds(5);
            //    return GetNmae(id);
            //});

            ////在今天晚上23点失效
            //strName = _cache.GetOrCreate<string>("Name", (e) => {
            //    e.AbsoluteExpiration = System.DateTime.Today.AddHours(23);
            //    return GetNmae(id);
            //});

            ////5秒后过期，假如在过期前被访问，以被访问时间点位基准再次延迟5秒，直到过期
            strName = _cache.GetOrCreate<string>("Name", (e) =>
            {
                e.SlidingExpiration = System.TimeSpan.FromSeconds(5);
                return GetNmae(id);
            });

            //混用绝对过期时间与滑动过期时间
            strName = _cache.GetOrCreate("Name", (e) =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);  //绝对过期时间15秒
                e.SlidingExpiration = TimeSpan.FromSeconds(5);  //滑动过期时间5秒
                return GetNmae(id);
            });
            #endregion
            return strName;
        }

        [HttpGet("Remove")]
        public void Remove(string cacheName)
        {
            _cache.Remove(cacheName);
        }

        private string GetNmae(int id)
        {
            switch (id)
            {
                case 1: return "Singo";
                case 2: return "古月";
                case 0: return null;
                default: return "无名";
            }
        }
    }
}
