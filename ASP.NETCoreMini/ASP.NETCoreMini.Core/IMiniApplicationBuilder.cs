using System;

namespace ASP.NETCoreMini.Core
{
    public interface IMiniApplicationBuilder
    {
        IMiniApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);
        RequestDelegate Build();
    }
}