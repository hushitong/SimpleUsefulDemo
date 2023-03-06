using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NETCoreMini.Core
{
    ///// <summary>
    ///// 简单的HttpContext
    ///// </summary>
    //public class MiniHttpContext
    //{
    //    public MiniHttpRequest Request { get; }
    //    public MiniHttpResponse Response { get; }
    //}

    ///// <summary>
    ///// 简单的HttpRequest
    ///// </summary>
    //public class MiniHttpRequest
    //{
    //    public Uri Url { get; }
    //    public NameValueCollection Headers { get; }
    //    public Stream Body { get; }
    //}
    ///// <summary>
    ///// 简单的HttpResponse
    ///// </summary>
    //public class MiniHttpResponse
    //{
    //    public NameValueCollection Headers { get; }
    //    public Stream Body { get; }
    //    public int StatusCode { get; set; }
    //}

    public class MiniHttpContext
    {
        public MiniHttpRequest Request { get; }
        public MiniHttpResponse Response { get; }

        public MiniHttpContext(IFeatureCollection features)
        {
            Request = new MiniHttpRequest(features);
            Response = new MiniHttpResponse(features);
        }
    }

    public class MiniHttpRequest
    {
        private readonly IHttpRequestFeature _feature;

        public Uri Url => _feature.Url;
        public NameValueCollection Headers => _feature.Headers;
        public Stream Body => _feature.Body;

        public MiniHttpRequest(IFeatureCollection features) => _feature = features.Get<IHttpRequestFeature>();
    }

    public class MiniHttpResponse
    {
        private readonly IHttpResponseFeature _feature;

        public NameValueCollection Headers => _feature.Headers;
        public Stream Body => _feature.Body;
        public int StatusCode { get => _feature.StatusCode; set => _feature.StatusCode = value; }

        public MiniHttpResponse(IFeatureCollection features) => _feature = features.Get<IHttpResponseFeature>();
    }

    public static partial class Extensions
    {
        public static Task WriteAsync(this MiniHttpResponse response, string contents)
        {
            var buffer = Encoding.UTF8.GetBytes(contents);
            return response.Body.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}