using Assets.Scripts.UI.Services.Windows;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private const string WaveChangerTag = "WaveChanger";
        private readonly IWindowService _windowService;

        public GameLoopState(IWindowService windowService) => 
            _windowService = windowService;
        public void Exit() { }

        public void Enter()
        {
            GameObject.FindWithTag(WaveChangerTag)
            .GetComponent<WaveChanger>()
                .Init(_windowService);
        }
    }
}