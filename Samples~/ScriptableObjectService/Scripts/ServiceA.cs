using Sapo.DI.Runtime.Attributes;
using UnityEngine;

namespace Sapo.DI.Samples.ScriptableObjectService
{
    [SRegister(typeof(IServiceA))]
    [CreateAssetMenu(menuName = "Sapo/DI/Samples/Scriptable Object Service/New Service A")]
    public class ServiceA : ScriptableObject, IServiceA
    {
        public void Introduce()
        {
            Debug.Log("[ServiceA] Hello, I am ServiceA!");
        }
    }
}
