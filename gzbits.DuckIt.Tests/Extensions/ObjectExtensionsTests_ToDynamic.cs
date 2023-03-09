using System.Collections;

using gzbits.DuckIt.Extensions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gzbits.DuckIt.Tests.Extensions
{
    [TestClass]
    public class ObjectExtensionsTests_ToDynamic
    {

        [TestMethod]
        public void EnumerableSchemaType_ThrowsNotSupportedException()
        {
            Type enumerableType = typeof(IEnumerable);

            // The value specified for the source object is irrelevant. This is testing the schema type.
            Assert.ThrowsException<NotSupportedException>(() =>
            {
                (new object()).ToDynamic(enumerableType);
            });

            Assert.ThrowsException<NotSupportedException>(() => (new object()).ToDynamic<string[]>());
        }

        [TestMethod]
        public void EnumerableObject_ThrowsNotSupportedException()
        {
            string[] strings = { "string" };

            // The value specified for the schema type is irrelevant. This is testing the source object's type
            Assert.ThrowsException<NotSupportedException>(() => strings.ToDynamic(typeof(object)));
            Assert.ThrowsException<NotSupportedException>(() => strings.ToDynamic<object>());
        }

        [TestClass]
        public class SinglePropertySchemas
        {
            [TestClass]
            public class HasMatchingPropertyNameAndReturnType
            {
                [TestMethod]
                public void StringProperty_ReturnsDynamicWithPropertyName()
                {
                    SourceObjects.SingleProperty.StringProperty objectWithStringProperty = new() { Value = "value" };

                    dynamic dynamicWithStringProperty = objectWithStringProperty.ToDynamic<Schemas.SingleProperty.StringProperty.ReadOnly>();
                    Assert.AreEqual("value", dynamicWithStringProperty?.Value);

                    dynamicWithStringProperty = objectWithStringProperty.ToDynamic<Schemas.SingleProperty.StringProperty.ReadWrite>();
                    Assert.AreEqual("value", dynamicWithStringProperty?.Value);
                }

                [TestMethod]
                public void IntegerProperty_ReturnsDynamicWithPropertyName()
                {
                    SourceObjects.SingleProperty.IntegerProperty objectWithIntegerProperty = new() { Value = 1 };

                    dynamic dynamicWithIntegerProperty = objectWithIntegerProperty.ToDynamic<Schemas.SingleProperty.ValueTypeProperty.IntegerProperty.ReadOnly>();
                    Assert.AreEqual(1, dynamicWithIntegerProperty?.Value);

                    dynamicWithIntegerProperty = objectWithIntegerProperty.ToDynamic<Schemas.SingleProperty.ValueTypeProperty.IntegerProperty.ReadWrite>();
                    Assert.AreEqual(1, dynamicWithIntegerProperty?.Value);
                }

                [TestMethod]
                public void EnumerableStringProperty_ReturnsDynamicWithPropertyName()
                {
                    string[] expectedValue = new[] { "string" };
                    SourceObjects.SingleProperty.EnumerableStringProperty objectWithEnumerableStringProperty = new() { Value = expectedValue };

                    dynamic dynamicWithEnumerableStringProperty = objectWithEnumerableStringProperty.ToDynamic<Schemas.SingleProperty.EnumerableProperty.EnumerableString.ReadOnly>();
                    Assert.AreEqual(expectedValue, dynamicWithEnumerableStringProperty.Value);

                    dynamicWithEnumerableStringProperty = objectWithEnumerableStringProperty.ToDynamic<Schemas.SingleProperty.EnumerableProperty.EnumerableString.ReadWrite>();
                    Assert.AreEqual(expectedValue, dynamicWithEnumerableStringProperty.Value);
                }
            }

            [TestClass]
            public class HasMatchingPropertyNameAndConvertableReturnType
            {

            }

            [TestClass]
            public class HasMatchingPropertyNameAndNonConvertableReturnType
            {

            }

            [TestClass]
            public class HasNoMatchingPropertyName
            {

            }
        }

        [TestClass]
        public class MultiplePropertySchemas
        {


        }

        private class SourceObjects
        {
            public class SingleProperty
            {
                public class StringProperty
                {
                    public string? Value { get; set; }
                }

                public class IntegerProperty
                {
                    public int? Value { get; set; }
                }

                public class EnumerableStringProperty
                {
                    public string[]? Value { get; set; }
                }
            }
        }

        private class Schemas
        {
            public class SingleProperty
            {
                public class StringProperty
                {
                    public interface ReadOnly
                    {
                        string Value { get; }
                    }

                    public interface ReadWrite
                    {
                        string Value { get; set; }
                    }
                }

                public class ValueTypeProperty
                {
                    public class IntegerProperty
                    {
                        public interface ReadOnly
                        {
                            int Value { get; }
                        }

                        public interface ReadWrite
                        {
                            int Value { get; set; }
                        }
                    }
                }

                public class EnumerableProperty
                {
                    public class EnumerableString
                    {
                        public interface ReadOnly
                        {
                            string[]? Value { get; }
                        }

                        public interface ReadWrite
                        {
                            string[]? Value { get; set; }
                        }
                    }
                }
            }
        }
    }
}
