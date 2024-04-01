using System;

namespace Sapo.DI.Samples.SceneLoading
{
    public interface IGameManager
    {
        event Action OnGameStart;
        
        void StartGame();

    }
}
