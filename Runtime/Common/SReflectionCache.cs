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
        public (Type componentType, Type[] registerTypes)[] RegistrableComponents { get; private set; }
        
        public Type[] InjectableComponents { get; private set; }

        private readonly Dictionary<Type, FieldInfo[]> _injectFieldsCache = new();

        public void Build()
        {
            var component = typeof(Component);

            var components = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.SafelyGetTypes())
                .Where(t => component.IsAssignableFrom(t));
            
            var registrableComponents = new List<(Type componentType, Type[] registerTypes)>();
            var injectableComponents = new List<Type>();
            
            foreach (var type in components)
            {
                if (type.IsDefinedWithAttribute<SRegister>(out var sRegister))
                    registrableComponents.Add((type, GetRegistererTypes(type, sRegister).ToArray()));

                var injectFields = type.GetInjectFields().ToArray();
                if (injectFields.IsEmpty()) continue;

                _injectFieldsCache[type] = injectFields;
                injectableComponents.Add(type);
            }
            
            RegistrableComponents = registrableComponents.ToArray();
            InjectableComponents = injectableComponents.ToArray();
        }
        
        private IEnumerable<Type> GetRegistererTypes(Type type, SRegister attribute)
        {
            if (attribute.Type == null) yield return type;
            else yield return attribute.Type;
            
            if (!attribute.RegisterAllInterfaces) yield break;

            foreach (var interfaceType in type.GetInterfaces()) yield return interfaceType;
        }
        
        public FieldInfo[] GetInjectFields(Type type)
        {
            if (_injectFieldsCache.TryGetValue(type, out var fields)) return fields;

            fields = ReflectInjectFields(type).ToArray();
            
            _injectFieldsCache[type] = fields;
            return fields;
        }

        private IEnumerable<FieldInfo> ReflectInjectFields(Type type)
        {
            var fields = type.GetInjectFields();
            
            var baseType = type.BaseType;
            if (baseType == null) return fields;
            if (baseType == typeof(object)) return fields;
            if (baseType == typeof(Object)) return fields;

            return fields.Concat(GetInjectFields(baseType));
        }
        
        
    }
}