using gzbits.DuckIt.Mvc.ResultExecutors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace gzbits.DuckIt.Mvc.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder UseDuckIt(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder?.Services?.AddSingleton<IActionResultExecutor<ObjectResult>, ProducesResponseTypeResultExecutor>();
            return mvcBuilder;
        }
    }
}
