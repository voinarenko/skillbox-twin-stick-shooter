using Assets.Scripts.UI.Services.Windows;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Elements.Buttons
{
    public class SettingsButton : Button
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            WindowService.Open(WindowId.Settings, StateMachine);
        }
    }
}