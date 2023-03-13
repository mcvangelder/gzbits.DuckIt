using gzbits.DuckIt.Converters;
using gzbits.DuckIt.Mvc.ResultExecutors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using gzbits.DuckIt.Mvc.OperationFilters;

namespace gzbits.DuckIt.Mvc.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder UseDuckIt(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.Services
                .AddSingleton<IActionResultExecutor<ObjectResult>, ProducesResponseTypeResultExecutor>()
                .AddSwaggerGen(config =>
                {
                    config.OperationFilter<ResponseProfileOperationFilter>();
                })
                .TryAddSingleton<IDuckItConverter, DuckItConverter>();

            return mvcBuilder;
        }
    }
}
