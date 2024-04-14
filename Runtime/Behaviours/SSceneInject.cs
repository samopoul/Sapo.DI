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
    [AddComponentMenu("Sapo/DI/Scene Inject")]
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
                Destroy();
                return;
            }
            
            var injector = SRootInjector.FindOrCreateSingleton();;
            injector.InjectScene(gameObject.scene);
            
            Destroy();
        }

        private void Destroy()
        {
            if (GetComponent<SRootInjector>() == null) Destroy(gameObject);
            else Destroy(this);
        }
    }
}