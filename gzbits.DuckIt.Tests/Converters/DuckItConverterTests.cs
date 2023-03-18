using gzbits.DuckIt.Converters;
using gzbits.DuckIt.Tests.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gzbits.DuckIt.Tests.Converters
{
    [TestClass]
    public class DuckItConverterTests
    {
        private static IDuckItConverter converter = new DuckItConverter(null);

        [TestClass]
        public class SinglePropertyTests
        {

            [TestMethod]
            public void IDuckItConverter_GenericConvert_SuccessfullyConverts()
            {
                Schemas.SingleProperty.ReadOnly.StringProperty? proxyInstance =
                    converter.Convert<SourceObjects.SingleProperty.StringProperty, Schemas.SingleProperty.ReadOnly.StringProperty>(
                        new SourceObjects.SingleProperty.StringProperty { Value = "test value" });

                Assert.IsNotNull(proxyInstance);
                Assert.AreEqual("test value", proxyInstance.Value);
            }

            [TestMethod]
            public void DuckItConverter_Convert_SuccessfullyConverts()
            {
                var proxyInstance =
                    converter.Convert(
                        new SourceObjects.SingleProperty.StringProperty { Value = "test value" }, typeof(Schemas.SingleProperty.ReadOnly.StringProperty)) as Schemas.SingleProperty.ReadOnly.StringProperty;

                Assert.IsNotNull(proxyInstance);
                Assert.AreEqual("test value", proxyInstance?.Value);
            }

            [TestMethod]
            public void DuckItConverter_GenericConvert_IntegerProperty_SuccessfullyConverts()
            {
                SourceObjects.SingleProperty.IntegerProperty objectWithIntegerProperty = new() { Value = 1 };

                var proxyInstance = converter.Convert<SourceObjects.SingleProperty.IntegerProperty, Schemas.SingleProperty.ReadOnly.ValueTypeProperty.IntegerProperty>(objectWithIntegerProperty);

                Assert.IsNotNull(proxyInstance);
                Assert.AreEqual(1, proxyInstance?.Value);
            }

            [TestMethod]
            public void DuckItConverter_GenericConvert_EnumerableStringProperty_SuccessfullyConverts()
            {
                string[] expectedValues = new[] { "value" };
                SourceObjects.SingleProperty.EnumerableStringProperty objectWithEnumerableStrings = new SourceObjects.SingleProperty.EnumerableStringProperty { Value = expectedValues };

                var proxyInstance = converter.Convert<SourceObjects.SingleProperty.EnumerableStringProperty, Schemas.SingleProperty.ReadOnly.EnumerableProperty.EnumerableStringProperty>(objectWithEnumerableStrings);

                Assert.IsNotNull(proxyInstance);
                Assert.AreEqual(expectedValues.Length, proxyInstance?.Value?.Length);
                Assert.AreEqual(expectedValues[0], proxyInstance?.Value?[0]);

            }
        }

        [TestClass]
        public class MultiPropertyTests
        {
            [TestMethod]
            public void DuckItConverter_GenericConvert_IntAndStringProperties_SuccessfullyConverts()
            {
                SourceObjects.MultiProperty.TwoProperties_Int_String objectWithIntAndStringProperty = new() { StringValue = "value", IntValue = 1 };

                var proxyInstance = converter.Convert<SourceObjects.MultiProperty.TwoProperties_Int_String, Schemas.MultiProperty.ReadOnly.TwoProperties_Int_String>(objectWithIntAndStringProperty);

                Assert.IsNotNull(proxyInstance);
                Assert.AreEqual(1, proxyInstance.IntValue);
                Assert.AreEqual("value", proxyInstance.StringValue);
            }
        }

        [TestClass]
        public class SupportedTypesTests
        {
            [TestMethod]
            public void DuckItConverter_Convert_InAndOutTypesAreSame_ReturnsSameInstance()
            {
                // The actual type and instance values are irrelevant, this is test is only concerned with whether the types are identical
                SourceObjects.SingleProperty.StringProperty source = new SourceObjects.SingleProperty.StringProperty { Value = "value" }; 
                Type outType = typeof(SourceObjects.SingleProperty.StringProperty);

                var instance = converter.Convert(source, outType);
                Assert.IsNotNull(instance);
                Assert.AreSame(source, instance);

                instance = converter.Convert<SourceObjects.SingleProperty.StringProperty, SourceObjects.SingleProperty.StringProperty>(source);
                Assert.IsNotNull(instance);
                Assert.AreSame(source, instance);
            }

            [TestMethod]
            public void DuckItConverter_Convert_OutputType_NotSameAndIsNotInterface_ThrowsDucktItException()
            {
                SourceObjects.SingleProperty.IntegerProperty source = new SourceObjects.SingleProperty.IntegerProperty {  Value = 1 };
                Type outType = typeof(SourceObjects.SingleProperty.StringProperty);

                DuckItException exception = Assert.ThrowsException<DuckItException>(() => converter.Convert(source, outType));
                Assert.IsInstanceOfType(exception.InnerException, typeof(ArgumentException));
            }
        }
    }
}
