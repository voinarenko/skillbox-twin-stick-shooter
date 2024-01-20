using UnityEngine;
using UnityEngine.UI;
using Button = Assets.Scripts.UI.Elements.Buttons.Button;

namespace Assets.Scripts.UI.Windows
{
    public class SettingsWindow : BaseWindow
    {
        public Button ConfirmButton;

        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _effectsSlider;

        public override void Init()
        {
            AudioService.UpdateSliders(_masterSlider, _musicSlider, _effectsSlider);
            ConfirmButton.Construct(SaveLoadService, AudioService, SettingsService);
        }
    }
}