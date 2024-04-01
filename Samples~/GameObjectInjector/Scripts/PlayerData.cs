using Sapo.DI.Runtime.Attributes;
using UnityEngine;

namespace Sapo.DI.Samples.ComponentService
{
    [SRegister(typeof(IPlayerData))]
    public class PlayerData : MonoBehaviour, IPlayerData
    {
        [SerializeField] private string playerName;

        public string PlayerName => playerName;
    }
}
