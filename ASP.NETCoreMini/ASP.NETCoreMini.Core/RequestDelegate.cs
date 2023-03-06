using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NETCoreMini.Core
{
    public delegate Task RequestDelegate(MiniHttpContext context);
}
