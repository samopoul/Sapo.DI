using System;
using System.Reflection;

namespace Sapo.DI.Runtime.Interfaces
{
    internal interface ISReflectionCache
    {
        (Type componentType, Type registerType)[] RegistrableComponents { get; }
        
        Type[] InjectableComponents { get; }
        
        void Build();
        
        FieldInfo[] GetInjectFields(Type type);

    }
}