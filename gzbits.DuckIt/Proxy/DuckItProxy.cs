using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gzbits.DuckIt.Proxy
{
    internal abstract class DuckItProxy
    {
        protected IDuckItProxyGenerator ProxyGenerator { get; private set; }
        public DuckItProxy(IDuckItProxyGenerator proxyGenerator)
        {
            this.ProxyGenerator = proxyGenerator;
        }
    }
}
