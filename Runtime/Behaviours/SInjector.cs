using System;
using System.Collections.Generic;
using Sapo.DI.Runtime.Common;
using Sapo.DI.Runtime.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sapo.DI.Runtime.Behaviours
{
    /// <summary>
    /// A simple implementation of <see cref="ISInjector"/> that uses reflection to inject dependencies.
    /// </summary>
    [AddComponentMenu("")]
    public class SInjector : MonoBehaviour, ISInjector
    {
        private static ISReflectionCache _reflectionCache;
        internal static ISReflectionCache ReflectionCache
        {
            get
            {
                if (_reflectionCache != null) return _reflectionCache;
                
                _reflectionCache = new SReflectionCache();
                _reflectionCache.Build();
                return _reflectionCache;
            }
        }

        private readonly Dictionary<Type, object> _instances = new();
        protected IEnumerable<object> RegisteredInstances => _instances.Values;


        public T Resolve<T>() => (T)Resolve(typeof(T));

        public object Resolve(Type type)
        {
            if (TryResolve(type, out var instance)) return instance;

            Debug.LogError($"[Sapo.DI] Unable to resolve <color=#ff8000>{type}</color> type. Make sure it's registered.");
            return null;
        }

        public bool TryResolve<T>(out T instance)
        {
            if (TryResolve(typeof(T), out var obj))
            {
                instance = (T)obj;
                return true;
            }

            instance = default;
            return false;
        }

        public bool TryResolve(Type type, out object instance)
        {
            if (!_instances.TryGetValue(type, out instance)) return false;
            
            var isObjectOrActiveUnityObject = instance != null && (instance is not Object o || o);
            if (isObjectOrActiveUnityObject) return true;
            

            _instances.Remove(type);
            instance = null;
            return false;
        }

        public bool IsRegistered<T>() => IsRegistered(typeof(T));

        public bool IsRegistered(Type type) => TryResolve(type, out _);


        public void Register<T>(object instance) => Register(typeof(T), instance);

        public void Register(Type type, object instance)
        {
            if (TryRegister(type, instance)) return;

            Debug.LogError($"[Sapo.DI] Unable to register <color=#ff8000>{type}</color> type. It's already registered.");
        }

        public bool TryRegister<T>(object instance) => TryRegister(typeof(T), instance);

        public bool TryRegister(Type type, object instance)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            if (IsRegistered(type)) return false;

            _instances[type] = instance;
            return true;
        }

        public void Unregister<T>(T instance) => Unregister(typeof(T), instance);
        
        public void Unregister(Type type, object instance)
        {
            if (!TryResolve(type, out var i)) return;
            if (i != instance) return;
            
            _instances.Remove(type);
        }

        public void Inject(object instance)
        {
            var type = instance.GetType();
            var fields = ReflectionCache.GetInjectFields(type);
            if (fields.IsEmpty()) return;

            foreach (var field in fields) field.SetValue(instance, Resolve(field.FieldType));
        }

        protected void PerformSelfInjection()
        {
            foreach (var instance in _instances.Values) 
                Inject(instance);
        }
    }
}