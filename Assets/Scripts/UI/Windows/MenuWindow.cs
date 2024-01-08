using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Windows
{
    public class MenuWindow : BaseWindow
    {
        public Button PlayButton;
        public Button SettingsButton;
        public Button QuitButton;

        protected override void OnAwake()
        {
            PlayButton.onClick.AddListener(()=>{});
            SettingsButton.onClick.AddListener(()=>{});
            QuitButton.onClick.AddListener(Application.Quit);
        }
    }
}