using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gzbits.DuckIt.Tests.Objects
{
    public class SourceObjects
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
}
