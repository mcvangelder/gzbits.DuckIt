﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using gzbits.DuckIt.Extensions;

namespace gzbits.DuckIt.Mvc.ResultExecutors
{
    public sealed class ProducesResponseTypeResultExecutor : ObjectResultExecutor
    {
        public ProducesResponseTypeResultExecutor(OutputFormatterSelector formatterSelector, IHttpResponseStreamWriterFactory writerFactory, ILoggerFactory loggerFactory, IOptions<MvcOptions> mvcOptions) 
            : base(formatterSelector, writerFactory, loggerFactory, mvcOptions)
        {
        }

        public override Task ExecuteAsync(ActionContext context, ObjectResult result)
        {
            ProducesResponseTypeAttribute? produceTypeAttribute = context.ActionDescriptor.EndpointMetadata.FirstOrDefault(meta => (meta as ProducesResponseTypeAttribute)?.StatusCode == (result.StatusCode ?? StatusCodes.Status200OK)) as ProducesResponseTypeAttribute;
            result.Value = result.Value?.ToDynamic(produceTypeAttribute?.Type ?? result.Value.GetType());
            return base.ExecuteAsync(context, result);
        }
    }
}