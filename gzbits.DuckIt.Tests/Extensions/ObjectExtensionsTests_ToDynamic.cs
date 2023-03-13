using System.Collections;
using System.Dynamic;

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
        public class HasNonMatchingProperty
        {
            [TestMethod]
            public void MissingPropertyNameOnSource_ReturnDynamicWithoutPropertyName()
            {
                SourceObjects.SingleProperty.StringProperty objectWithStringProperty = new() { Value = "value" };

                dynamic dynamicWithOutProperties = objectWithStringProperty.ToDynamic<Schemas.MultiProperty.ReadOnly.TwoProperties_Int_String>();
                Assert.AreEqual(0, (dynamicWithOutProperties as ExpandoObject)?.Count());
            }

            [TestMethod]
            public void NonMatchingPropertyType_ReturnsDynamicWithoutPropertyName()
            {
                SourceObjects.SingleProperty.StringProperty objectWithStringProperty = new() { Value = "value" };

                dynamic dynamicWithoutProperties = objectWithStringProperty.ToDynamic<Schemas.SingleProperty.ReadOnly.ValueTypeProperty.IntegerProperty>();
                Assert.AreEqual(0, (dynamicWithoutProperties as ExpandoObject)?.Count());
            }
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

                    dynamic dynamicWithStringProperty = objectWithStringProperty.ToDynamic<Schemas.SingleProperty.ReadOnly.StringProperty>();
                    Assert.AreEqual("value", dynamicWithStringProperty?.Value);
                    Assert.AreEqual(1, (dynamicWithStringProperty as ExpandoObject)?.Count());

                    dynamicWithStringProperty = objectWithStringProperty.ToDynamic<Schemas.SingleProperty.ReadWrite.StringProperty>();
                    Assert.AreEqual("value", dynamicWithStringProperty?.Value);
                    Assert.AreEqual(1, (dynamicWithStringProperty as ExpandoObject)?.Count());
                }

                [TestMethod]
                public void IntegerProperty_ReturnsDynamicWithPropertyName()
                {
                    SourceObjects.SingleProperty.IntegerProperty objectWithIntegerProperty = new() { Value = 1 };

                    dynamic dynamicWithIntegerProperty = objectWithIntegerProperty.ToDynamic<Schemas.SingleProperty.ReadOnly.ValueTypeProperty.IntegerProperty>();
                    Assert.AreEqual(1, dynamicWithIntegerProperty?.Value);
                    Assert.AreEqual(1, (dynamicWithIntegerProperty as ExpandoObject)?.Count());

                    dynamicWithIntegerProperty = objectWithIntegerProperty.ToDynamic<Schemas.SingleProperty.ReadWrite.ValueTypeProperty.IntegerProperty>();
                    Assert.AreEqual(1, dynamicWithIntegerProperty?.Value);
                    Assert.AreEqual(1, (dynamicWithIntegerProperty as ExpandoObject)?.Count());
                }

                [TestMethod]
                public void EnumerableStringProperty_ReturnsDynamicWithPropertyName()
                {
                    string[] expectedValue = new[] { "string" };
                    SourceObjects.SingleProperty.EnumerableStringProperty objectWithEnumerableStringProperty = new() { Value = expectedValue };

                    dynamic dynamicWithEnumerableStringProperty = objectWithEnumerableStringProperty.ToDynamic<Schemas.SingleProperty.ReadOnly.EnumerableProperty.EnumerableStringProperty>();
                    Assert.AreEqual(1, (dynamicWithEnumerableStringProperty as ExpandoObject)?.Count());
                    Assert.AreEqual(expectedValue[0], dynamicWithEnumerableStringProperty.Value[0]);

                    dynamicWithEnumerableStringProperty = objectWithEnumerableStringProperty.ToDynamic<Schemas.SingleProperty.ReadWrite.EnumerableProperty.EnumerableStringProperty>();
                    Assert.AreEqual(1, (dynamicWithEnumerableStringProperty as ExpandoObject)?.Count());
                    Assert.AreEqual(expectedValue[0], dynamicWithEnumerableStringProperty.Value[0]);
                }


                [TestMethod]
                public void ObjectProperty_ThrowsNotSupportedException()
                {
                    SourceObjects.SingleProperty.ObjectProperty objectWithObjectProperty = new() { Value = new object() };
                    Assert.ThrowsException<NotSupportedException>(() =>
                    {
                        objectWithObjectProperty.ToDynamic<Schemas.SingleProperty.ReadOnly.ObjectProperty>();
                    });

                }

                [TestMethod]
                public void EnumerableNonValueTypeProperty_ThrowsNotSupportedException()
                {
                    SourceObjects.SingleProperty.EnumerableObjectProperty objectWithObjectProperty = new() { Values = new SourceObjects[] { new SourceObjects() } };
                    Assert.ThrowsException<NotSupportedException>(() =>
                    {
                        objectWithObjectProperty.ToDynamic<Schemas.SingleProperty.ReadOnly.EnumerableProperty.EnumerableObjectProperty>();
                    });
                    
                    // TODO: Add tests for unsupported enumerable contents: e.g. an empty array of objects
                }

            }
        }

        [TestClass]
        public class MultiplePropertySchemas
        {

            [TestMethod]
            public void IntAndStringProperty_ReturnsDynamicWithPropertyNames()
            {
                SourceObjects.MultiProperty.TwoProperties_Int_String objectWithIntAndStringProperty = new() { StringValue = "value", IntValue = 1 };

                dynamic dynamicWithIntAndStringProperty = objectWithIntAndStringProperty.ToDynamic<Schemas.MultiProperty.ReadOnly.TwoProperties_Int_String>();
                Assert.AreEqual("value", dynamicWithIntAndStringProperty?.StringValue);
                Assert.AreEqual(1, dynamicWithIntAndStringProperty?.IntValue);
                Assert.AreEqual(2, (dynamicWithIntAndStringProperty as ExpandoObject)?.Count());

                dynamicWithIntAndStringProperty = objectWithIntAndStringProperty.ToDynamic<Schemas.MultiProperty.ReadWrite.TwoProperties_Int_String>();
                Assert.AreEqual("value", dynamicWithIntAndStringProperty?.StringValue);
                Assert.AreEqual(1, dynamicWithIntAndStringProperty?.IntValue);
                Assert.AreEqual(2, (dynamicWithIntAndStringProperty as ExpandoObject)?.Count());
            }
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

                public class ObjectProperty
                {
                    public object? Value { get; set; }
                }

                public class EnumerableObjectProperty
                {
                    public object[]? Values { get; set; }
                }
            }

            public class MultiProperty
            {
                public class TwoProperties_Int_String
                {
                    public int IntValue { get; set; }
                    public string? StringValue { get; set; }
                }
            }
        }

        private class Schemas
        {
            public class SingleProperty
            {
                public class ReadOnly
                {
                    public interface StringProperty
                    {
                        string Value { get; }
                    }

                    public class ValueTypeProperty
                    {
                        public interface IntegerProperty
                        {
                            int? Value { get; }
                        }
                    }

                    public interface ObjectProperty
                    {
                        object? Value { get; }
                    }

                    public class EnumerableProperty
                    {
                        public interface EnumerableStringProperty
                        {
                            string[]? Value { get; }
                        }

                        public interface EnumerableObjectProperty
                        {
                            object[]? Values { get; }
                        }
                    }
                }

                public class ReadWrite
                {
                    public interface StringProperty
                    {
                        string Value { get; set; }
                    }

                    public class ValueTypeProperty
                    {
                        public interface IntegerProperty
                        {
                            int? Value { get; set; }
                        }
                    }
                    public class EnumerableProperty
                    {
                        public interface EnumerableStringProperty
                        {
                            string[]? Value { get; set; }
                        }
                    }
                }
            }

            public class MultiProperty
            {
                public class ReadOnly
                {
                    public interface TwoProperties_Int_String
                    {
                        int IntValue { get; }
                        string StringValue { get; }
                    }
                }

                public class ReadWrite
                {
                    public interface TwoProperties_Int_String
                    {
                        int IntValue { get; set; }
                        string StringValue { get; set; }
                    }
                }
            }
        }
    }
}
