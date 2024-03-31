using Sapo.DI.Runtime.Attributes;
using UnityEngine;

namespace Sapo.DI.Samples.ScriptableObjectService
{
    public class Player : MonoBehaviour
    {
        [SInject] private ServiceC _serviceC;

        private void Awake()
        {
            Debug.Log("[Player] Hello, I am Player! I am referencing service C: ");
            _serviceC.Introduce();
        }
    }   
}