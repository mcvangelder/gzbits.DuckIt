using gzbits.DuckIt.Proxy;
using gzbits.DuckIt.Tests.Objects;
using System.Collections;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;

namespace gzbits.DuckIt.Tests.Proxy
{
    [TestClass]
    public class DynamicTypeBuilderTests
    {
        internal static DynamicTypeBuilder typeBuilder = new DynamicTypeBuilder();

        [TestMethod]
        public void EnumerableSchemaType_ThrowsNotSupportedException()
        {
            Type enumerableType = typeof(IEnumerable);

            // The value specified for the source object is irrelevant. This is testing the schema type.
            Assert.ThrowsException<ArgumentException>(() => typeBuilder.GetOrCreateProxyType(new object(), enumerableType));
        }

        [TestMethod]
        public void EnumerableObject_ThrowsNotSupportedException()
        {
            string[] strings = { "string" };

            // The value specified for the schema type is irrelevant. This is testing the source object's type
            Assert.ThrowsException<ArgumentException>(() => typeBuilder.GetOrCreateProxyType(strings, typeof(object)));
        }

        [TestClass]
        public class HasNonMatchingProperty
        {
            [TestMethod]
            public void MissingPropertyNameOnSource_ThrowsDuckItException_MissingMemberException()
            {
                SourceObjects.SingleProperty.StringProperty objectWithStringProperty = new() { Value = "value" };

                DuckItException ex = Assert.ThrowsException<DuckItException>(() => typeBuilder.GetOrCreateProxyType(objectWithStringProperty, typeof(Schemas.MultiProperty.ReadOnly.TwoProperties_Int_String)));
                Assert.IsInstanceOfType(ex.InnerException, typeof(MissingMemberException));
            }

            [TestMethod]
            public void NonMatchingPropertyType_ThrowsDuckItException_NotSupportedException()
            {
                SourceObjects.SingleProperty.StringProperty objectWithStringProperty = new() { Value = "value" };

                DuckItException ex = Assert.ThrowsException<DuckItException>(() => typeBuilder.GetOrCreateProxyType(objectWithStringProperty, typeof(Schemas.SingleProperty.ReadOnly.ValueTypeProperty.IntegerProperty)));
                Assert.IsInstanceOfType(ex.InnerException, typeof(NotSupportedException));
            }
        }

        [TestClass]
        public class SinglePropertySchemas
        {
            [TestClass]
            public class HasMatchingPropertyNameAndReturnType
            {
                [TestMethod]
                public void StringProperty_ReturnsTypeWithPropertyName()
                {
                    SourceObjects.SingleProperty.StringProperty objectWithStringProperty = new() { Value = "value" };

                    Type schemaType = typeof(Schemas.SingleProperty.ReadOnly.StringProperty);
                    Type proxyType = typeBuilder.GetOrCreateProxyType(objectWithStringProperty, schemaType);

                    AssertProxyIsDuckItProxy(proxyType);
                    AssertProxyMatchesSchema(proxyType, schemaType);

                }

                [TestMethod]
                public void EnumerableStringProperty_ReturnsTypeWithPropertyName()
                {
                    string[] expectedValue = new[] { "string" };
                    SourceObjects.SingleProperty.EnumerableStringProperty objectWithEnumerableStringProperty = new() { Value = expectedValue };

                    Type schemaType = typeof(Schemas.SingleProperty.ReadOnly.EnumerableProperty.EnumerableStringProperty);
                    Type proxyType = typeBuilder.GetOrCreateProxyType(objectWithEnumerableStringProperty, schemaType);

                    AssertProxyIsDuckItProxy(proxyType);
                    AssertProxyMatchesSchema(proxyType, schemaType);
                }


                [TestMethod]
                public void ObjectProperty_TypeWithPropertyName()
                {
                    SourceObjects.SingleProperty.ObjectProperty objectWithObjectProperty = new() { Value = new object() };

                    Type schemaType = typeof(Schemas.SingleProperty.ReadOnly.ObjectProperty);
                    Type proxyType = typeBuilder.GetOrCreateProxyType(objectWithObjectProperty, schemaType);

                    AssertProxyIsDuckItProxy(proxyType);
                    AssertProxyMatchesSchema(proxyType, schemaType);
                }

                [TestMethod]
                public void EnumerableNonValueTypeProperty_ThrowsNotSupportedException()
                {
                    SourceObjects.SingleProperty.EnumerableObjectProperty objectWithObjectProperty = new() { Values = new SourceObjects[] { new SourceObjects() } };
                    Type schemaType = typeof(Schemas.SingleProperty.ReadOnly.EnumerableProperty.EnumerableObjectProperty);
                    Type proxyType = typeBuilder.GetOrCreateProxyType(objectWithObjectProperty, schemaType);

                    AssertProxyIsDuckItProxy(proxyType);
                    AssertProxyMatchesSchema(proxyType, schemaType);
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

                Type schemaType = typeof(Schemas.MultiProperty.ReadOnly.TwoProperties_Int_String);
                Type proxyType = typeBuilder.GetOrCreateProxyType(objectWithIntAndStringProperty, schemaType);

                AssertProxyIsDuckItProxy(proxyType);
                AssertProxyMatchesSchema(proxyType, schemaType);
            }
        }

        private static void AssertProxyIsDuckItProxy(Type proxyType)
        {
            Type duckItProxyType = typeof(DuckItProxy);
            Assert.AreEqual(duckItProxyType, proxyType.BaseType);
        }

        private static void AssertProxyMatchesSchema(Type proxyType, Type schemaType)
        {
            Assert.AreEqual(schemaType, proxyType.GetInterfaces()[0]);
            Assert.AreEqual(proxyType.GetProperties().Length, schemaType.GetProperties().Length);
        }
    }
}
