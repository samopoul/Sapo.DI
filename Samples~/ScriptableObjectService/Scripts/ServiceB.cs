using Sapo.DI.Runtime.Attributes;
using UnityEngine;

namespace Sapo.DI.Samples.ScriptableObjectService
{
    [SRegister(typeof(ServiceB))]
    [CreateAssetMenu(menuName = "Sapo/DI/Samples/Scriptable Object Service/New Service B")]
    public class ServiceB : ScriptableObject
    {
        public void Introduce()
        {
            Debug.Log("[ServiceB] Hello, I am ServiceB!");
        }
    }
}
