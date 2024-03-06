using Assets.Scripts.Infrastructure.Services.Wave;
using Assets.Scripts.UI.Services.Windows;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private const string WaveChangerTag = "WaveChanger";
        private readonly IWindowService _windowService;
        private readonly IWaveService _waveService;
        private readonly SceneLoader _sceneLoader;

        public GameLoopState(SceneLoader sceneLoader, IWindowService windowService, IWaveService waveService)
        {
            _sceneLoader = sceneLoader;
            _windowService = windowService;
            _waveService = waveService;
        }

        public void Enter()
        {
            var manager = GameObject.FindWithTag(WaveChangerTag);
            manager.GetComponent<WaveChanger>().Init(_sceneLoader);
            manager.GetComponent<PauseListener>().Init(_windowService, _sceneLoader);
        }

        public void Exit() => 
            _waveService.SpawnPoints?.Clear();
    }
}