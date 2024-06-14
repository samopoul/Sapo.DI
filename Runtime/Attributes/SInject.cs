using System;

namespace Sapo.DI.Runtime.Attributes
{
    /// <summary>
    /// The SInject attribute is used to mark fields that should be injected.
    /// </summary>
    /// <example>
    /// This sample shows how to use the SInject attribute.
    /// <code>
    /// public class MyClass
    /// {
    ///     [SInject] private MyDependency _myDependency;
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class SInject : Attribute
    {
        
    }
}