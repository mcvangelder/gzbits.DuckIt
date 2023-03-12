using gzbits.DuckIt.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gzbits.DuckIt.Converters
{
    public interface IDuckItConverter
    {
        dynamic Convert<Tout, Tin>(Tin source)
            where Tout : class
            where Tin : notnull
        {
            return Convert(source, typeof(Tout));
        }

        dynamic Convert<Tin>(Tin source, Type outType) where Tin : notnull;
    }

    public class DuckItConverter : IDuckItConverter
    {
        private readonly ILogger<DuckItConverter>? logger;

        public DuckItConverter(ILogger<DuckItConverter> logger)
        {
            this.logger = logger;
        }

        public dynamic Convert<Tin>(Tin source, Type outType) where Tin : notnull
        {
            logger?.LogDebug($"Converting {typeof(Tin)} to {outType.GetType()}");
            return source.ToDynamic(outType);
        }
    }
}
