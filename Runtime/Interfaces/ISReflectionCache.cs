using System;
using System.Reflection;

namespace Sapo.SInject.Runtime.Interfaces
{
    internal interface ISReflectionCache
    {
        (Type componentType, Type registerType)[] RegistrableComponents { get; }
        
        Type[] InjectableComponents { get; }
        
        void Build();
        
        FieldInfo[] GetInjectFields(Type type);

    }
}