using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using gzbits.DuckIt.Extensions;

namespace gzbits.DuckIt.Proxy
{
    internal class DynamicTypeBuilder
    {
        private readonly static ModuleBuilder _moduleBuilder;
        private static readonly object _lockObject = new();
        private static readonly ConcurrentBag<string> _generatedTypeNames = new ConcurrentBag<string>();

        static DynamicTypeBuilder()
        {
            _moduleBuilder = GetOrCreateModule(currentBuilder: _moduleBuilder);
        }

        public Type GetOrCreateProxyType<Tin>(Tin source, Type interfaceType) where Tin : class
        {
            if (!interfaceType.IsInterface) { throw new ArgumentException($"Non-interface type {interfaceType.FullName} is not supported.", nameof(interfaceType)); }
            if (interfaceType.IsAssignableTo(typeof(IEnumerable)) || source.GetType().IsAssignableTo(typeof(IEnumerable)))
            {
                throw new ArgumentException($"Enumerable sources are not supported. Type: {source.GetType()}");
            }

            string proxyTypeName = $"{interfaceType.Name}${source.GetType().Name}";
            if(_generatedTypeNames.Contains(proxyTypeName)) { return _moduleBuilder.GetType(proxyTypeName) ?? throw new DuckItException("Unexpected error. Generated Type is null."); }

            TypeBuilder builder = CreateTypeBuilder(proxyTypeName);
            Type proxyType = BuildProxyType(builder, source, interfaceType) ?? throw new DuckItException("Unexpected error. Generated Type is null.");
            _generatedTypeNames.Add(proxyTypeName);
            return proxyType;
        }

        private static TypeBuilder CreateTypeBuilder(string proxyTypeName)
        {
            TypeBuilder typeBuilder = _moduleBuilder.DefineType(proxyTypeName, TypeAttributes.Public | TypeAttributes.Sealed);

            return typeBuilder;
        }

        private static ModuleBuilder GetOrCreateModule(ModuleBuilder currentBuilder)
        {
            var builder = currentBuilder;
            if (builder == null)
            {
                lock (_lockObject)
                {
                    builder = currentBuilder ?? CreateNewModuleBuilder();
                }
            }

            return builder;

            static ModuleBuilder CreateNewModuleBuilder()
            {
                var assemblyName = new AssemblyName("gzbits.DuckIt$DynamicAssembly");
                AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(nameof(DynamicTypeBuilder));
                return moduleBuilder;
            }
        }

        private static Type? BuildProxyType<Tin>(TypeBuilder builder, Tin source, Type interfaceType) where Tin : class
        {

            builder.SetParent(typeof(DuckItProxy));
            builder.AddInterfaceImplementation(interfaceType);

            AddConstructorAndSourceField(builder, source, out FieldBuilder sourceFieldBuilder);
            AddPropertyProxies(builder, source, sourceFieldBuilder, interfaceType);

            return builder.CreateType();
        }

        private static void AddConstructorAndSourceField<Tin>(TypeBuilder builder, Tin source, out FieldBuilder sourceFieldBuilder) where Tin : class
        {
            sourceFieldBuilder = AddSourceField(builder, source);
            ConstructorBuilder constructorBuilder = builder.DefineConstructor(
                MethodAttributes.Public |
                    MethodAttributes.HideBySig |
                    MethodAttributes.SpecialName |
                    MethodAttributes.RTSpecialName,
                CallingConventions.HasThis,
                new[] { source.GetType(), typeof(IDuckItProxyGenerator) });
            constructorBuilder.DefineParameter(1, ParameterAttributes.None, "source");
            constructorBuilder.DefineParameter(2, ParameterAttributes.None, "proxyGenerator");


            ConstructorInfo proxyGeneratorConstructor = typeof(DuckItProxy).GetConstructor(new[] { typeof(IDuckItProxyGenerator) });

            ILGenerator emitter = constructorBuilder.GetILGenerator();
            emitter.Emit(OpCodes.Ldarg_0);
            emitter.Emit(OpCodes.Ldarg_2);
            emitter.Emit(OpCodes.Call, proxyGeneratorConstructor);
            emitter.Emit(OpCodes.Nop);
            emitter.Emit(OpCodes.Nop);
            emitter.Emit(OpCodes.Ldarg_0);
            emitter.Emit(OpCodes.Ldarg_1);
            emitter.Emit(OpCodes.Stfld, sourceFieldBuilder);
            emitter.Emit(OpCodes.Ret);
        }

        private static FieldBuilder AddSourceField<Tin>(TypeBuilder builder, Tin source) where Tin : class =>
            builder.DefineField(nameof(source), source.GetType(), FieldAttributes.Private | FieldAttributes.InitOnly);

        private static void AddPropertyProxies<Tin>(TypeBuilder builder, Tin source, FieldBuilder sourceFieldBuider, Type interfaceType) where Tin : class
        {
            IDictionary<string, PropertyInfo> sourceProperties = source.GetType().GetReadableProperties();
            foreach (var prop in interfaceType.GetReadableProperties().Values)
            {
                // TODO: Handle enumerable properties

                if (sourceProperties.TryGetValue(prop.Name, out PropertyInfo sourcePropertyInfo))
                {
                    if (prop.PropertyType != sourcePropertyInfo.PropertyType)
                    {
                        ThrowPropertyMismatchException(typeof(Tin), interfaceType, new NotSupportedException($"Source type '{sourcePropertyInfo.PropertyType}' must be '{prop.PropertyType}'"));
                    }

                    CreateProperty(builder, sourceFieldBuider, prop, sourcePropertyInfo);
                }
                else
                {
                    ThrowPropertyMismatchException(typeof(Tin), interfaceType, new MissingMemberException(typeof(Tin).FullName, prop.Name));
                }
            }
        }

        private static void CreateProperty(TypeBuilder builder, FieldBuilder sourceFieldBuider, PropertyInfo prop, PropertyInfo sourcePropertyInfo)
        {
            PropertyBuilder propertyBuilder = builder.DefineProperty(prop.Name, PropertyAttributes.None, prop.PropertyType, Type.EmptyTypes);
            MethodBuilder propertyMethod = builder.DefineMethod($"get_{prop.Name}",
                MethodAttributes.Public |
                    MethodAttributes.Final |
                    MethodAttributes.HideBySig |
                    MethodAttributes.SpecialName |
                    MethodAttributes.NewSlot |
                    MethodAttributes.Virtual,
                CallingConventions.HasThis, prop.PropertyType, Type.EmptyTypes);
            ILGenerator propertyMethodEmitter = propertyMethod.GetILGenerator();
            propertyMethodEmitter.Emit(OpCodes.Ldarg_0);
            propertyMethodEmitter.Emit(OpCodes.Ldfld, sourceFieldBuider);
            propertyMethodEmitter.Emit(OpCodes.Call, sourcePropertyInfo.GetMethod);
            propertyMethodEmitter.Emit(OpCodes.Ret);
            propertyBuilder.SetGetMethod(propertyMethod);
        }

        private static void ThrowPropertyMismatchException(Type sourceType, Type interfaceType, Exception inner) =>
            throw new DuckItException($"Source object '{sourceType}' must have matching properties with schema '{interfaceType}'. See inner excpetion for details.", inner);
    }
}
