using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sapo.SInject.Runtime.Common
{
    internal static class ReflectionExtensions
    {
        internal static bool IsDefinedWithAttribute<T>(this MemberInfo member) where T : Attribute =>
            Attribute.GetCustomAttribute(member, typeof(T)) != null;
        
        internal static bool IsDefinedWithAttribute<T>(this MemberInfo member, out T attribute) where T : Attribute
        {
            attribute = GetAttribute<T>(member);
            return attribute != null;
        }

        internal static T GetAttribute<T>(this MemberInfo member) where T : Attribute =>
            (T)Attribute.GetCustomAttribute(member, typeof(T));

        internal static IEnumerable<FieldInfo> GetInjectFields(this Type type)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                       BindingFlags.FlattenHierarchy;

            return type.GetFields(flags).Where(f => f.IsDefinedWithAttribute<Attributes.SInject>());
        }
    }
}