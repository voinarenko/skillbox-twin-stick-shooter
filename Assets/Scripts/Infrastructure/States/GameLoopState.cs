using Assets.Scripts.UI.Services.Windows;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private const string WaveChangerTag = "WaveChanger";
        private readonly SceneLoader _sceneLoader;

        public GameLoopState(SceneLoader sceneLoader) => 
            _sceneLoader = sceneLoader;

        public void Exit() { }

        public void Enter()
        {
            GameObject.FindWithTag(WaveChangerTag)
            .GetComponent<WaveChanger>()
                .Init(_sceneLoader);
        }
    }
}