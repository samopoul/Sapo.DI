using Sapo.DI.Runtime.Attributes;
using UnityEngine;

namespace Sapo.DI.Samples.Testing.Runtime
{
    public class Player : MonoBehaviour
    {
        [SInject] private IHealth _health;
        
        public void TakeDamage(int damage)
        {
            _health.Value -= damage;
        }

    }
}