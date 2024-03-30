using System.ComponentModel;
using UnityEngine;

namespace Sapo.DI.Runtime.Behaviours
{
    [HelpURL("https://github.com/sapo-creations/sk.sapo.dependency-injection")]
    [DisplayName("GameObject Inject")]
    [AddComponentMenu("Sapo/DI/GameObject Inject")]
    public class SGameObjectInject : MonoBehaviour
    {
        private void Awake()
        {
            var injector = FindObjectOfType<SRootInjector>();
            if (injector == null)
            {
                Debug.LogError("[Sapo.DI] Unable to inject gameObject, no SInjector found.");
                Destroy(this);
                return;
            }

            injector.InjectGameObject(gameObject);
            Destroy(this);
        }
    }
}