using Assets.Scripts.UI.Elements.Buttons;

namespace Assets.Scripts.UI.Windows
{
    public class MenuWindow : BaseWindow
    {
        public Button SettingsButton;

        public override void Init() => 
            AudioService.GetVolume();
    }
}