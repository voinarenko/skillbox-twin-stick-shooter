using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Elements.Buttons
{
    public class SettingsButton : Button
    {
        public override void OnPointerClick(PointerEventData eventData) => UiFactory.CreateSettings();
    }
}