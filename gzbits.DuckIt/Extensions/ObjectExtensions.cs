using System.Collections;
using System.Diagnostics;
using System.Dynamic;
using System.Reflection;

namespace gzbits.DuckIt.Extensions
{
    public static class ObjectExtensions
    {
        public static dynamic ToDynamic<TSchema>(this object obj)
        {
            return obj.ToDynamic(typeof(TSchema));
        }

        public static dynamic ToDynamic(this object obj, Type schemaType)
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

                if (inputProperties.ContainsKey(propertyName))
                {
                    PropertyInfo inputProperty = inputProperties[propertyName];
                    Type inputPropertyType = inputProperty.PropertyType;
                    object? inputValue = inputProperty.GetValue(obj);
                    if (inputPropertyType.IsValueType || inputPropertyType.IsAssignableTo(typeof(string)))
                    {
                        object? outputValue = inputValue;
                        if (property.PropertyType != inputPropertyType)
                        {
                            // a property is considred a match if name and type are a match. skip this property
                            // as this is not considered a match
                            continue;
                        }
                        outPutType.TryAdd(propertyName, outputValue);
                    }
                    else
                    {
                        var enumerable = inputValue as IEnumerable;
                        if (enumerable != null)
                        {
                            foreach (var item in enumerable)
                            {
                                if (item is ValueType || item is string)
                                {
                                    object? outputValue = inputValue;
                                    if (property.PropertyType != inputPropertyType)
                                    {
                                        // a property is considred a match if name and type are a match. skip this property
                                        // as this is not considered a match
                                        continue;
                                    }
                                    outPutType.TryAdd(propertyName, outputValue);
                                }
                            }
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
