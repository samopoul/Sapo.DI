using System;

namespace Sapo.DI.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class SRegister : Attribute
    {
        public Type Type { get; }
        
        public SRegister(Type type) => Type = type;
    }
}