using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sapo.SInject.Runtime.Attributes;
using Sapo.SInject.Runtime.Common;
using Sapo.SInject.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sapo.SInject.Runtime.Behaviours
{
    /// <summary>
    /// A root injector that initializes the dependency injection system and injects dependencies in the scene.
    /// </summary>
    [HelpURL("https://github.com/sapo-creations/sk.sapo.dependency-injection")]
    [DisplayName("Root Injector")]
    [AddComponentMenu("Sapo/DI/Root Injector")]
    public sealed class SRootInjector : SInjector
    {
        [SerializeField] private bool makePersistent = true;
        [SerializeField] private List<Object> assetsToRegister = new();
        
        
        private static SRootInjector _instance;
        
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_instance != null)
            {
                if (_instance == this) return;
                
                Destroy(gameObject);
                return;
            }
            
            if (makePersistent) DontDestroyOnLoad(gameObject);
            _instance = this;
            
            foreach (var rootInjector in FindObjectsOfType<SRootInjector>())
            {
                if (rootInjector == this) continue;
                
                Destroy(rootInjector);
            }
            
            foreach (var asset in assetsToRegister)
            {
                if (asset == null) continue;
                if (!asset.GetType().IsDefinedWithAttribute<SRegister>(out var sRegister)) continue;
                
                Register(sRegister.Type, asset);
            }

            foreach (var handler in RegisteredInstances.OfType<ISInjectorRegisterHandler>()) handler.OnRegister(this);
            
            PerformSelfInjection();

            foreach (var handler in RegisteredInstances.OfType<ISInjectorInjectHandler>()) handler.OnInject(this);
        }
        
        internal void InjectScene(Scene scene)
        {
            Initialize();
            var reflectionCache = ReflectionCache;
            
            var registerHandlers = new List<ISInjectorRegisterHandler>();
            var injectHandlers = new List<ISInjectorInjectHandler>();
            var roots = scene.GetRootGameObjects();
            
            foreach (var root in roots)
            {
                foreach (var (componentT, registerT) in reflectionCache.RegistrableComponents)
                foreach (var component in root.GetComponentsInChildren(componentT, true))
                    Register(registerT, component);

                registerHandlers.AddRange(root.GetComponentsInChildren<ISInjectorRegisterHandler>(true));
                injectHandlers.AddRange(root.GetComponentsInChildren<ISInjectorInjectHandler>(true));
            }

            foreach (var handler in registerHandlers) handler.OnRegister(this);
            
            foreach (var root in roots)
            foreach (var injectableComponentT in reflectionCache.InjectableComponents)
            foreach (var component in root.GetComponentsInChildren(injectableComponentT, true))
                Inject(component);
            
            foreach (var handler in injectHandlers) handler.OnInject(this);
        }
        
        internal void InjectGameObject(GameObject obj)
        {
            Initialize();
            var reflectionCache = ReflectionCache;

            var registerHandlers = obj.GetComponentsInChildren<ISInjectorRegisterHandler>(true);
            var injectHandlers = obj.GetComponentsInChildren<ISInjectorInjectHandler>(true);
            
            foreach (var (componentT, registerT) in reflectionCache.RegistrableComponents)
            foreach (var component in obj.GetComponentsInChildren(componentT, true))
                Register(registerT, component);

            foreach (var handler in registerHandlers) handler.OnRegister(this);
            
            foreach (var injectableComponentT in reflectionCache.InjectableComponents)
            foreach (var component in obj.GetComponentsInChildren(injectableComponentT, true))
                Inject(component);
            
            foreach (var handler in injectHandlers) handler.OnInject(this);
        }
    }
}