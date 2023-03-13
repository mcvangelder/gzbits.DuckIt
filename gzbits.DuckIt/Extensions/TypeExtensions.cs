using System.Reflection;

namespace gzbits.DuckIt.Extensions
{
    internal static class TypeExtensions
    {
        internal static IDictionary<string, PropertyInfo> GetReadableProperties(this Type type)
        {
            return type.GetProperties().Where(info => info.CanRead).ToDictionary((prop) => prop.Name);
        }

        internal static IDictionary<string, PropertyInfo> GetWritablePropertiesAsDictionary(this Type type)
        {
            return type.GetProperties().Where(info => info.CanWrite).ToDictionary((prop) => prop.Name);
        }

        /// <summary>
        /// Determines if the type is a simple type: Enum, ValueType (int, double, float, long, DateTime, etc) or assignable to string
        /// </summary>
        /// <param name="type">The type to check if is primitive</param>
        /// <returns>true if type represents an enum, value type, or assignable to string</returns>
        internal static bool IsSimpleType(this Type type)
        {
            return type.IsEnum || type.IsValueType || type.IsAssignableTo(typeof(string));
        }

    }
}
