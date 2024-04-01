using System;
using UnityEngine;
using Sapo.DI.Runtime.Behaviours;
using Sapo.DI.Runtime.Interfaces;

namespace Sapo.DI.Tests.Runtime.Helpers
{
    /// <summary>
    /// A helper class for injecting game objects for testing purposes.
    /// </summary>
    public class STestGamObjectInjector : ISInjector
    {
        private readonly GameObject _gameObject;
        private readonly ISInjector _injector;

        /// <summary>
        /// Creates a <see cref="SGameObjectInjector"/> on the given game object.
        /// Please make sure that the game object is so-called 'uninitialized' that means that
        /// is was disabled before adding components to it. Otherwise, the injector will not inject
        /// the game object.
        /// </summary>
        /// <param name="gameObject">The game object to inject.</param>
        public STestGamObjectInjector(GameObject gameObject)
        {
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            if (gameObject.activeInHierarchy) throw new ArgumentException("The game object must be disabled.", nameof(gameObject));
            if (gameObject.GetComponent<SGameObjectInjector>() != null)
                throw new InvalidOperationException("The game object already has an injector.");
            
            _gameObject = gameObject;
            var injector = gameObject.AddComponent<SGameObjectInjector>();
            _injector = injector.Injector;
            gameObject.AddComponent<SPrefabInject>();
        }


        public T Resolve<T>() => _injector.Resolve<T>();

        public object Resolve(Type type) => _injector.Resolve(type);

        public bool TryResolve<T>(out T instance) => _injector.TryResolve(out instance);

        public bool TryResolve(Type type, out object instance) => _injector.TryResolve(type, out instance);

        public bool IsRegistered<T>() => _injector.IsRegistered<T>();

        public bool IsRegistered(Type type) => _injector.IsRegistered(type);

        public void Register<T>(object instance) => _injector.Register<T>(instance);

        public void Register(Type type, object instance) => _injector.Register(type, instance);

        public bool TryRegister<T>(object instance) => _injector.TryRegister<T>(instance);

        public bool TryRegister(Type type, object instance) => _injector.TryRegister(type, instance);

        public void Unregister<T>(object instance) => _injector.Unregister<T>(instance);

        public void Unregister(Type type, object instance) => _injector.Unregister(type, instance);

        public void Inject(object instance) => _injector.Inject(instance);
        
        
        /// <summary>
        /// Activates the game object.
        /// </summary>
        public void Activate() => _gameObject.SetActive(true);
    }
}