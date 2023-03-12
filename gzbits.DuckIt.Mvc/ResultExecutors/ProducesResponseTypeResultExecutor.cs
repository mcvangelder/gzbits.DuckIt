using gzbits.DuckIt.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;

namespace gzbits.DuckIt.Mvc.ResultExecutors
{
    public sealed class ProducesResponseTypeResultExecutor : ObjectResultExecutor
    {
        private readonly IDuckItConverter duckItConverter;

        public ProducesResponseTypeResultExecutor(IDuckItConverter duckItConverter,
            OutputFormatterSelector formatterSelector,
            IHttpResponseStreamWriterFactory writerFactory,
            ILoggerFactory loggerFactory,
            IOptions<MvcOptions> mvcOptions): base(formatterSelector,writerFactory,loggerFactory,mvcOptions)
        {
            this.duckItConverter = duckItConverter;
        }

        public override Task ExecuteAsync(ActionContext context, ObjectResult result)
        {
            ProducesResponseTypeAttribute? produceTypeAttribute = context.ActionDescriptor.EndpointMetadata.FirstOrDefault(meta => (meta as ProducesResponseTypeAttribute)?.StatusCode == (result.StatusCode ?? StatusCodes.Status200OK)) as ProducesResponseTypeAttribute;
            if (result.Value is not null)
            {
                result.Value = duckItConverter.Convert(result.Value, produceTypeAttribute?.Type ?? result.Value.GetType());
            }
            return base.ExecuteAsync(context, result);
        }
    }
}
