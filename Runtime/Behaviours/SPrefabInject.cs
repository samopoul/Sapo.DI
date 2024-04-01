using System.ComponentModel;
using Sapo.DI.Runtime.Attributes;
using Sapo.DI.Runtime.Interfaces;
using UnityEngine;

namespace Sapo.DI.Runtime.Behaviours
{
    /// <summary>
    /// A GameObject Inject is a component that injects dependencies in the GameObject during game object instantiation.
    /// </summary>
    [HelpURL("https://github.com/sapo-creations/sk.sapo.dependency-injection")]
    [DisplayName("Prefab Inject")]
    [AddComponentMenu("Sapo/DI/Prefab Inject")]
    [DisallowMultipleComponent]
    public sealed class SPrefabInject : MonoBehaviour, ISInjectorRegisterHandler
    {
        private bool _isInjected;
        
        void ISInjectorRegisterHandler.OnRegister(ISInjector injector) => _isInjected = true;

        private void Awake()
        {
            if (_isInjected)
            {
                Destroy(this);
                return;
            }
            
            var injector = FindObjectOfType<SRootInjector>();
            if (injector != null)
            {
                injector.InjectGameObject(gameObject);
                Destroy(this);
                return;
            }

            var localInjector = GetComponent<SGameObjectInjector>();
            if (localInjector != null)
            {
                localInjector.Inject();
                Destroy(this);
                return;
            }


            Debug.LogError("[Sapo.DI] Unable to inject gameObject, no SInjector found.");
            Destroy(this);
        }

    }
}