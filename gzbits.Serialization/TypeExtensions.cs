using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace gzbits.Serialization
{
    public static class TypeExtensions
    {
        public static IDictionary<string, PropertyInfo> GetReadableProperties(this Type type)
        {
            return type.GetProperties().Where(info => info.CanRead).ToDictionary((prop) => prop.Name);
        }

        public static IDictionary<string, PropertyInfo> GetWritablePropertiesAsDictionary(this Type type)
        {
            return type.GetProperties().Where(info => info.CanWrite).ToDictionary((prop) => prop.Name);
        }
    }
}
