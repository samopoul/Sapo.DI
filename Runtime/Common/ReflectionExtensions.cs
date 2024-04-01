using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sapo.DI.Runtime.Common
{
    internal static class ReflectionExtensions
    {
        internal static bool IsDefinedWithAttribute<T>(this MemberInfo member) where T : Attribute =>
            Attribute.GetCustomAttribute(member, typeof(T)) != null;
        
        internal static bool IsDefinedWithAttribute<T>(this MemberInfo member, out T attribute) where T : Attribute
        {
            attribute = GetDefinedAttribute<T>(member);
            return attribute != null;
        }

        internal static T GetDefinedAttribute<T>(this MemberInfo member) where T : Attribute =>
            (T)Attribute.GetCustomAttribute(member, typeof(T));
        
        internal static bool IsDefinedWithAttribute<T>(this Type type) where T : Attribute =>
            type.GetDefinedAttribute<T>() != null;

        internal static bool IsDefinedWithAttribute<T>(this Type type, out T attribute) where T : Attribute
        {
            attribute = GetDefinedAttribute<T>(type);
            return attribute != null;
        }
        
        internal static T GetDefinedAttribute<T>(this Type type) where T : Attribute
        {
            while (type != null && type != typeof(object))
            {
                var attribute = (T)type.GetCustomAttribute(typeof(T), false);
                if (attribute != null) return attribute;
                type = type.BaseType;
            }

            return null;
        }

        internal static IEnumerable<FieldInfo> GetInjectFields(this Type type)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                       BindingFlags.FlattenHierarchy;

            return type.GetFields(flags).Where(f => f.IsDefinedWithAttribute<Attributes.SInject>());
        }
        
        internal static IEnumerable<Type> SafelyGetTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (Exception e)
            {
                // ignore
            }

            return Type.EmptyTypes;
        }
    }
}