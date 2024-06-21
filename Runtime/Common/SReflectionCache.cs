using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sapo.DI.Runtime.Attributes;
using Sapo.DI.Runtime.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sapo.DI.Runtime.Common
{
    internal class SReflectionCache : ISReflectionCache
    {
        private Dictionary<Type, Type[]> _registrableTypesCache = new();
        
        public (Type componentType, Type[] registerTypes)[] RegistrableComponents { get; private set; }
        
        public Type[] InjectableComponents { get; private set; }

        private readonly Dictionary<Type, FieldInfo[]> _injectFieldsCache = new();

        public void Build()
        {
            var components = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.SafelyGetTypes());
            
            var registrableComponents = new List<(Type componentType, Type[] registerTypes)>();
            var injectableComponents = new List<Type>();
            var componentType = typeof(Component);
            
            foreach (var type in components)
            {
                var isComponent = componentType.IsAssignableFrom(type);
                var isRegistrable = type.IsDefinedWithAttribute<SRegister>();
                
                if (isRegistrable) _registrableTypesCache[type] = ReflectRegisterTypes(type).ToArray();
                if (isRegistrable && isComponent) registrableComponents.Add((type, GetRegisterTypes(type)));

                var injectFields = ReflectInjectFields(type).ToArray();
                if (injectFields.IsEmpty()) continue;

                _injectFieldsCache[type] = injectFields;
                if (isComponent) injectableComponents.Add(type);
            }
            
            RegistrableComponents = registrableComponents.ToArray();
            InjectableComponents = injectableComponents.ToArray();
        }

        public Type[] GetRegisterTypes(Type type) =>
            _registrableTypesCache.GetValueOrDefault(type, Array.Empty<Type>());

        private IEnumerable<Type> ReflectRegisterTypes(Type type)
        {
            var result = Enumerable.Empty<Type>();
            
            foreach (var attribute in type.GetCustomAttributes<SRegister>())
            {
                if (!attribute.RegisterAllInterfaces)
                {
                    result = result.Append(attribute.Type ?? type);
                    continue;
                }

                if (attribute.Type != null) result = result.Append(attribute.Type);
                result = result.Concat(type.GetInterfaces());
            }

            var baseType = type.BaseType;
            if (CannotReflect(baseType)) return result;

            result = result.Concat(ReflectRegisterTypes(baseType));
            return result.Distinct();
        }

        public FieldInfo[] GetInjectFields(Type type) =>
            _injectFieldsCache.GetValueOrDefault(type, Array.Empty<FieldInfo>());

        private IEnumerable<FieldInfo> ReflectInjectFields(Type type)
        {
            var fields = type.GetInjectFields();
            
            var baseType = type.BaseType;
            if (CannotReflect(baseType)) return fields;
            
            return fields.Concat(ReflectInjectFields(baseType));
        }

        private bool CannotReflect(Type type) => type == null || type == typeof(object) || type == typeof(Object);


    }
}