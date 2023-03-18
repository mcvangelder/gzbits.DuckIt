using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gzbits.DuckIt.Tests.Objects
{
    public class Schemas
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
