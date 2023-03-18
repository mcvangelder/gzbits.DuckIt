using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gzbits.DuckIt.Proxy
{
    internal interface IDuckItProxyGenerator
    {
        object? CreateProxyInstance(object source, Type type);
    }

    internal class DuckItProxyGenerator : IDuckItProxyGenerator
    {
        private DynamicTypeBuilder TypeBuilder { get; set; }

        public DuckItProxyGenerator(DynamicTypeBuilder typeBuilder)
        {
            TypeBuilder = typeBuilder;
        }

        public object? CreateProxyInstance(object source, Type interfaceType)
        { 
            Type proxyType = TypeBuilder.GetOrCreateProxyType(source, interfaceType);
            return Activator.CreateInstance(proxyType, source, this);
        }
    }
}
