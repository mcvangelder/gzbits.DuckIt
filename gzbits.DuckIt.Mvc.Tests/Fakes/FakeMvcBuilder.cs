using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace gzbits.DuckIt.Mvc.Tests.Fakes
{
    internal class FakeMvcBuilder : IMvcBuilder
    {
        public IServiceCollection Services { get; } = new ServiceCollection();

        public ApplicationPartManager PartManager => throw new NotImplementedException();
    }
}