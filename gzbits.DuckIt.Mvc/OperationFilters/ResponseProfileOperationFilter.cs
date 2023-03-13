using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace gzbits.DuckIt.Mvc.OperationFilters
{
    public class ResponseProfileOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation?.Parameters.Add(new OpenApiParameter()
            {
                Name = "x-duckit-response-schema",
                In = ParameterLocation.Header,
                Required = false
            });
        }
    }
}
