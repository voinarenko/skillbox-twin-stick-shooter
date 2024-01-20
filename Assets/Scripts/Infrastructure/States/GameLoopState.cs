using Assets.Scripts.UI.Services.Windows;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private const string WaveChangerTag = "WaveChanger";
        private readonly IWindowService _windowService;
        private readonly SceneLoader _sceneLoader;

        public GameLoopState(SceneLoader sceneLoader, IWindowService windowService)
        {
            _sceneLoader = sceneLoader;
            _windowService = windowService;
        }

        public void Exit() { }

        public void Enter()
        {
            var manager = GameObject.FindWithTag(WaveChangerTag);
            manager.GetComponent<WaveChanger>().Init(_sceneLoader);
            manager.GetComponent<PauseListener>().Init(_windowService, _sceneLoader);
        }
    }
}