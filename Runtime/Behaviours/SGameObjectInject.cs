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
    [DisplayName("GameObject Inject")]
    [AddComponentMenu("Sapo/DI/GameObject Inject")]
    [DisallowMultipleComponent]
    public sealed class SGameObjectInject : MonoBehaviour, ISInjectorRegisterHandler
    {
        private bool _skip;
        
        
        void ISInjectorRegisterHandler.OnRegister(ISInjector injector) => _skip = true;

        private void Awake()
        {
            if (_skip)
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

            var localInjector = FindObjectOfType<SGameObjectInjector>();
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