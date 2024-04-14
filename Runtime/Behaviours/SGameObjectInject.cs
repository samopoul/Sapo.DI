using System.ComponentModel;
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
        [SerializeField] private bool localInjector = false;
        internal bool CreateLocalInjector
        {
            get => localInjector;
            set => localInjector = value;
        }

        private bool _isInjected;
        
        void ISInjectorRegisterHandler.OnRegister(ISInjector injector) => _isInjected = true;

        private void Awake()
        {
            if (_isInjected)
            {
                Destroy(this);
                return;
            }

            var injector = SRootInjector.FindOrCreateSingleton();
            injector.InjectGameObject(gameObject);
            
            Destroy(this);
        }

    }
}