using Assets.Scripts.Data;
using Assets.Scripts.UI.Windows;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Elements.Buttons
{
    public class ConfirmButton : Button
    {
        private BaseWindow Window => GetComponentInParent<BaseWindow>();

        public override void OnPointerClick(PointerEventData eventData)
        {
            SettingsService.Settings = new Settings();
            AudioService.StoreVolume(SettingsService.Settings);
            SaveLoadService.SaveSettings();
            Destroy(Window.gameObject);
        }
    }
}