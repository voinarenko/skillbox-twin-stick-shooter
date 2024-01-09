using Assets.Scripts.Infrastructure.States;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Elements.Buttons
{
    public class PlayButton : Button
    {
        public override void OnPointerClick(PointerEventData eventData) => StateMachine.Enter<LoadProgressState>();
    }
}