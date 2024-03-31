using System.ComponentModel;
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
    public sealed class SSceneInject : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("[Sapo.DI] Injecting scene.");
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