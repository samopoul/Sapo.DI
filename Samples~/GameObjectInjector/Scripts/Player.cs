using Sapo.DI.Runtime.Attributes;
using UnityEngine;

namespace Sapo.DI.Samples.ComponentService
{
    public class Player : MonoBehaviour
    {
        [SInject] private IPlayerData _playerData;
        
        private void Start()
        {
            name = $"Player ({_playerData.PlayerName})";
        }
    }
}
