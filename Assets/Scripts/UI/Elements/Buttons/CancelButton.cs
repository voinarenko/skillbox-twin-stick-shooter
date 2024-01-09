using Assets.Scripts.UI.Windows;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Elements.Buttons
{
    public class CancelButton : Button
    {
        private BaseWindow Window => GetComponentInParent<BaseWindow>();

        public override void OnPointerClick(PointerEventData eventData)
        {
            AudioService.CancelChanges();
            Destroy(Window.gameObject);
        }
    }
}