using System;
using System.Collections.Generic;

namespace ASP.NETCoreMini.Core
{
    public class MiniWbeHostBuilder : IMiniWebHostBuilder
    {
        private IMiniServer _server;
        private readonly List<Action<IMiniApplicationBuilder>> _configures = new List<Action<IMiniApplicationBuilder>>();

        public IMiniWebHostBuilder Configure(Action<IMiniApplicationBuilder> configure)
        {
            _configures.Add(configure);
            return this;
        }
        public IMiniWebHostBuilder UseServer(IMiniServer server)
        {
            _server = server;
            return this;
        }

        public IMiniWebHost Build()
        {
            var builder = new MiniApplicationBuilder();
            foreach (var configure in _configures)
            {
                configure.Invoke(builder);
            }
            return new MiniWebHost(_server, builder.Build());
        }
    }
}
