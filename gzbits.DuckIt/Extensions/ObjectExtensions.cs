using System.Collections;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection;

namespace gzbits.DuckIt.Extensions
{
    internal static class ObjectExtensions
    {
        internal static dynamic ToDynamic<TSchema>(this object obj) where TSchema : class
        {
            return obj.ToDynamic(typeof(TSchema));
        }

        internal static dynamic ToDynamic(this object obj, Type schemaType)
        {
            Type inputType = (obj ?? new object()).GetType();
            if (inputType.IsAssignableTo(typeof(IEnumerable)) || schemaType.IsAssignableTo(typeof(IEnumerable)))
            {
                throw new NotSupportedException($"One of the following is enumerable and is not supported: {inputType.FullName}, {schemaType.FullName}");
            }


            IDictionary<string, PropertyInfo> schemaProperties = schemaType.GetReadableProperties();
            IDictionary<string, PropertyInfo> inputProperties = inputType.GetReadableProperties();

            IDictionary<string, object?> outPutType = new ExpandoObject();

            foreach (PropertyInfo property in schemaProperties.Values)
            {
                string propertyName = property.Name;
                if (inputProperties.TryGetValue(propertyName, out PropertyInfo? inputProperty) && property.PropertyType == inputProperty.PropertyType)
                {
                    Type inputPropertyType = inputProperty.PropertyType;
                    object? inputValue = inputProperty.GetValue(obj);
                    if (inputPropertyType.IsSimpleType())
                    {
                        outPutType.TryAdd(propertyName, inputValue);
                    }
                    else
                    {
                        if (inputValue is IEnumerable enumerable)
                        {
                            List<object> enumerableValues = new ();
                            foreach (var item in enumerable)
                            {
                                if (item.GetType().IsSimpleType())
                                {
                                    enumerableValues.Add(item);
                                }
                                else
                                {
                                    if (item is not null)
                                    {
                                        throw new NotSupportedException($"{item.GetType().FullName} is not supported.");
                                    }
                                }
                            }
                            outPutType.TryAdd(propertyName, enumerableValues);
                        }
                        else
                        {
                            throw new NotSupportedException(inputPropertyType.FullName);
                        }
                    }
                }
            }

            return outPutType;
        }
    }
}
