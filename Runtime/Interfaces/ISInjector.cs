using System;
using Sapo.DI.Runtime.Attributes;

namespace Sapo.DI.Runtime.Interfaces
{
    /// <summary>
    /// A simple interface for a sapo injector.
    /// </summary>
    public interface ISInjector
    {
        //public ISInjector Parent { get; }
        
        /// <summary>
        /// Resolves the instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the instance to resolve.</typeparam>
        /// <returns>The resolved instance of the specified type.</returns>
        public T Resolve<T>();
        
        /// <summary>
        /// Resolves the instance of the specified type.
        /// </summary>
        /// <param name="type">The type of the instance to resolve.</param>
        /// <returns>The resolved instance of the specified type.</returns>
        public object Resolve(Type type);
        
        
        /// <summary>
        /// Tries to resolve the instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the instance to resolve.</typeparam>
        /// <param name="instance">The resolved instance of the specified type if the operation is successful; otherwise, default value of T.</param>
        /// <returns>true if the operation is successful; otherwise, false.</returns>
        public bool TryResolve<T>(out T instance);
        
        /// <summary>
        /// Tries to resolve the instance of the specified type.
        /// </summary>
        /// <param name="type">The type of the instance to resolve.</param>
        /// <param name="instance">The resolved instance of the specified type if the operation is successful; otherwise, null.</param>
        /// <returns>true if the operation is successful; otherwise, false.</returns>
        public bool TryResolve(Type type, out object instance);

        /// <summary>
        /// Checks if the specified type is registered.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns>true if the type is registered; otherwise, false.</returns>
        public bool IsRegistered<T>();
        
        /// <summary>
        /// Checks if the specified type is registered.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is registered; otherwise, false.</returns>
        public bool IsRegistered(Type type);
        
        /// <summary>
        /// Registers an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the instance to register.</typeparam>
        /// <param name="instance">The instance to register.</param>
        public void Register<T>(object instance);
        
        /// <summary>
        /// Registers an instance of the specified type.
        /// </summary>
        /// <param name="type">The type of the instance to register.</param>
        /// <param name="instance">The instance to register.</param>
        public void Register(Type type, object instance);

        /// <summary>
        /// Tries to register an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the instance to register.</typeparam>
        /// <param name="instance">The instance to register.</param>
        /// <returns>true if the operation is successful; otherwise, false.</returns>
        public bool TryRegister<T>(object instance);
        
        /// <summary>
        /// Tries to register an instance of the specified type.
        /// </summary>
        /// <param name="type">The type of the instance to register.</param>
        /// <param name="instance">The instance to register.</param>
        /// <returns>true if the operation is successful; otherwise, false.</returns>
        public bool TryRegister(Type type, object instance);
        
        /// <summary>
        /// Unregisters the specified instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the instance to unregister.</typeparam>
        /// <param name="instance">The instance to unregister.</param>
        public void Unregister<T>(object instance);
        
        /// <summary>
        /// Unregisters the specified instance of the specified type.
        /// </summary>
        /// <param name="type">The type of the instance to unregister.</param>
        /// <param name="instance">The instance to unregister.</param>
        public void Unregister(Type type, object instance);

        /// <summary>
        /// Injects dependencies into the specified instance. All fields defined with <see cref="SInjectAttribute"/> will be injected.
        /// </summary>
        /// <param name="instance">The instance to inject.</param>
        public void Inject(object instance);
    }
}
