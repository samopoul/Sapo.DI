using System.ComponentModel;
using Sapo.DI.Runtime.Interfaces;
using UnityEngine;

namespace Sapo.DI.Runtime.Behaviours
{
    /// <summary>
    /// Scene Inject is a component that injects entire scene during scene load.
    /// </summary>
    [HelpURL("https://github.com/sapo-creations/sk.sapo.dependency-injection")]
    [DisplayName("Scene Inject")]
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    public sealed class SSceneInject : MonoBehaviour, ISInjectorRegisterHandler
    {
        private bool _isInjected;
        
        void ISInjectorRegisterHandler.OnRegister(ISInjector injector) => _isInjected = true;
        
        private void Awake()
        {
            if (_isInjected)
            {
                Debug.LogError("[Sapo.DI] Scene already injected. Make sure you have only one SSceneInject per scene.");
                Destroy(gameObject);
                return;
            }
            
            var injector = FindObjectOfType<SRootInjector>();
            if (injector == null)
            {
                Debug.LogError("[Sapo.DI] Unable to inject scene, no SInjector found.");
                Destroy(gameObject);
                return;
            }

            injector.InjectScene(gameObject.scene);
            Destroy(gameObject);
        }
    }
}