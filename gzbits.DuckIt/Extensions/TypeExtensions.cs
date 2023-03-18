using System.Reflection;

namespace gzbits.DuckIt.Extensions
{
    internal static class TypeExtensions
    {
        internal static IDictionary<string, PropertyInfo> GetReadableProperties(this Type type)
        {
            return type.GetProperties().Where(info => info.CanRead).ToDictionary((prop) => prop.Name);
        }

        internal static IDictionary<string, PropertyInfo> GetWritableProperties(this Type type)
        {
            return type.GetProperties().Where(info => info.CanWrite).ToDictionary((prop) => prop.Name);
        }
    }
}
