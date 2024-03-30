using System;

namespace Sapo.DI.Runtime.Interfaces
{
    public interface ISInjector
    {
        //public ISInjector Parent { get; }
        
        public T Resolve<T>();
        public object Resolve(Type type);
        
        public bool TryResolve<T>(out T instance);
        public bool TryResolve(Type type, out object instance);

        public bool IsRegistered<T>();
        public bool IsRegistered(Type type);
        
        public void Register<T>(object instance);
        public void Register(Type type, object instance);

        public bool TryRegister<T>(object instance);
        public bool TryRegister(Type type, object instance);
        
        public void Unregister<T>(T instance);
        public void Unregister(Type type, object instance);

        public void Inject(object instance);
    }
}
