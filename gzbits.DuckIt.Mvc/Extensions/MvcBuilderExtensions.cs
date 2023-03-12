using gzbits.DuckIt.Converters;
using gzbits.DuckIt.Mvc.ResultExecutors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace gzbits.DuckIt.Mvc.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder UseDuckIt(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.Services
                .AddSingleton<IActionResultExecutor<ObjectResult>, ProducesResponseTypeResultExecutor>()
                .TryAddSingleton<IDuckItConverter, DuckItConverter>();

            return mvcBuilder;
        }
    }
}
