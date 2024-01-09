using Assets.Scripts.UI.Elements.Buttons;

namespace Assets.Scripts.UI.Windows
{
    public class SettingsWindow : BaseWindow
    {
        public Button ConfirmButton;
        public Button CancelButton;

        public void Init()
        {
            AudioService.GetVolume();
            ConfirmButton.Construct(SaveLoadService, AudioService);
            CancelButton.Construct(AudioService);
        }
    }
}