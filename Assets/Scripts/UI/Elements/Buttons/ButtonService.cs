using Assets.Scripts.Infrastructure.States;
using UnityEngine;

namespace Assets.Scripts.UI.Elements.Buttons
{
    public class ButtonService : IButtonService
    {
        private readonly IGameStateMachine _stateMachine;

        public ButtonService(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void StartGame()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

        public void OpenSettings()
        {
            
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}