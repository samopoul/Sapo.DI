using System.ComponentModel;
using UnityEngine;

namespace Sapo.SInject.Runtime.Behaviours
{
    /// <summary>
    /// Scene Inject is a component that injects entire scene during scene load.
    /// </summary>
    [HelpURL("https://github.com/sapo-creations/sk.sapo.dependency-injection")]
    [DisplayName("Scene Inject")]
    [AddComponentMenu("Sapo/DI/Scene Inject")]
    public class SSceneInject : MonoBehaviour
    {
        private void Awake()
        {
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