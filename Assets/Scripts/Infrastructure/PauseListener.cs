using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.UI.Services.Windows;
using Assets.Scripts.UI.Windows;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class PauseListener : MonoBehaviour
    {
        private PauseWindow _window;
        private IGameStateMachine _stateMachine;
        private IWindowService _windowService;
        private SceneLoader _sceneLoader;
        private bool _paused;

        public void Construct(IGameStateMachine stateMachine) => 
            _stateMachine = stateMachine;

        public void Init(IWindowService windowService, SceneLoader sceneLoader)
        {
            _windowService = windowService;
            _sceneLoader = sceneLoader;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_paused)
                {
                    _windowService.Open(WindowId.Pause, _stateMachine);
                    _window = FindAnyObjectByType<PauseWindow>();
                    _window.SetLoader(_sceneLoader);
                    _paused = true;
                }
                else
                {
                    Destroy(_window.gameObject);
                    _paused = false;
                }
            }
        }

        public void CancelPause() => 
            _paused = false;
    }
}