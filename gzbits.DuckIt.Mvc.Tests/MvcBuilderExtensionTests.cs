using gzbits.DuckIt.Converters;
using gzbits.DuckIt.Mvc.Extensions;
using gzbits.DuckIt.Mvc.ResultExecutors;
using gzbits.DuckIt.Mvc.Tests.Fakes;
using Microsoft.Extensions.DependencyInjection;

namespace gzbits.DuckIt.Mvc.Tests
{
    [TestClass]
    public class MvcBuilderExtensionTests
    {
        private static IMvcBuilder _mvcBuilder = new FakeMvcBuilder();

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _mvcBuilder.UseDuckIt();
        }

        [TestMethod]
        public void UseDuckItExtension_AddsProducesResponseTypeResultExecutor()
        {
            Assert.IsNotNull(_mvcBuilder.Services.SingleOrDefault(sd => sd.ImplementationType == typeof(ProducesResponseTypeResultExecutor)));
        }

        [TestMethod]
        public void UseDuckItExtension_AddsDuckItConverter()
        {
            Assert.IsNotNull(_mvcBuilder.Services.SingleOrDefault(sd => sd.ImplementationType == typeof(DuckItConverter)));
        }
    }
}