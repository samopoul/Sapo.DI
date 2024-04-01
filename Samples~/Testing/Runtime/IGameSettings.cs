using System;

namespace Sapo.DI.Samples.Testing.Runtime
{
    public interface IGameSoundSettings
    {
        public float MusicVolume { get; set; }
        
        public event Action OnChanged;
    }
}