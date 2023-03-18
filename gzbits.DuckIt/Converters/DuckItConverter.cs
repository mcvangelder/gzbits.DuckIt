using gzbits.DuckIt.Extensions;
using gzbits.DuckIt.Proxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gzbits.DuckIt.Converters
{
    public interface IDuckItConverter
    {
        Tout? Convert<Tin, Tout>(Tin source)
            where Tout : class
            where Tin : class
        {
            return Convert(source, typeof(Tout));
        }

        dynamic? Convert<Tin>(Tin source, Type outType) where Tin : class;
    }

    public class DuckItConverter : IDuckItConverter
    {
        private readonly ILogger<DuckItConverter>? logger;
        private readonly IDuckItProxyGenerator duckItProxyGenerator = new DuckItProxyGenerator(new DynamicTypeBuilder());

        public DuckItConverter(ILogger<DuckItConverter>? logger)
        {
            this.logger = logger;
        }

        public object? Convert<Tin>(Tin source, Type outType) where Tin : class
        {
            if(source.GetType() == outType) { return source; }
            if (!outType.IsInterface) { throw new DuckItException($"Provided type must be an interface. See inner exception for details.", new ArgumentException($"Not an interface: {outType}.", nameof(outType))); }

            logger?.LogDebug($"Converting {typeof(Tin)} to {outType.GetType()}");
            return duckItProxyGenerator.CreateProxyInstance(source, outType);
        }
    }
}
