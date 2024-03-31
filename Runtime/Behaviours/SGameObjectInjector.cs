using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sapo.DI.Runtime.Attributes;
using Sapo.DI.Runtime.Common;
using Sapo.DI.Runtime.Interfaces;
using UnityEngine;

namespace Sapo.DI.Runtime.Behaviours
{
    [HelpURL("https://github.com/sapo-creations/sk.sapo.dependency-injection")]
    [DisplayName("GameObject Injector")]
    [AddComponentMenu("Sapo/DI/GameObject Injector")]
    [DisallowMultipleComponent]
    public sealed class SGameObjectInjector : MonoBehaviour
    {
        [SerializeField] private List<Object> assetsToRegister = new();

        private SInjector _injector;
        internal ISInjector Injector
        {
            get
            {
                Initialize();
                return _injector;
            }
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_injector != null) return;
            
            _injector = new SInjector(FindObjectOfType<SRootInjector>()?.Injector);
            
            foreach (var asset in assetsToRegister)
            {
                if (asset == null) continue;
                if (!asset.GetType().IsDefinedWithAttribute<SRegister>(out var sRegister)) continue;
                
                _injector.Register(sRegister.Type, asset);
            }

            foreach (var handler in _injector.RegisteredInstances.OfType<ISInjectorRegisterHandler>()) handler.OnRegister(_injector);
            
            _injector.PerformSelfInjection();

            foreach (var handler in _injector.RegisteredInstances.OfType<ISInjectorInjectHandler>()) handler.OnInject(_injector);
        }

        internal void Inject()
        {
            Initialize();
            var reflectionCache = SInjector.ReflectionCache;

            var registerHandlers = GetComponents<ISInjectorRegisterHandler>();
            var injectHandlers = GetComponents<ISInjectorInjectHandler>();
            
            foreach (var (componentT, registerT) in reflectionCache.RegistrableComponents)
            foreach (var component in GetComponents(componentT))
                _injector.Register(registerT, component);

            foreach (var handler in registerHandlers) handler.OnRegister(_injector);
            
            foreach (var injectableComponentT in reflectionCache.InjectableComponents)
            foreach (var component in GetComponents(injectableComponentT))
                _injector.Inject(component);
            
            foreach (var handler in injectHandlers) handler.OnInject(_injector);
        }
    }
}