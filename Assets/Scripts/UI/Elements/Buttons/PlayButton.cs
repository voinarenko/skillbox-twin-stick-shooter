using Assets.Scripts.Infrastructure.States;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Assets.Scripts.UI.Elements.Buttons
{
    public class PlayButton : Button
    {
        private IGameStateMachine _stateMachine;

        [Inject]
        public void Construct(IGameStateMachine stateMachine) => _stateMachine = stateMachine;

        public override void OnPointerClick(PointerEventData eventData) => Debug.Log($"Play -{_stateMachine}-");
    }
}