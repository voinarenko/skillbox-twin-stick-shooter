using System.Threading.Tasks;
using Assets.Scripts.UI.Services.Factory;
using UnityEngine;

namespace Assets.Scripts.UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IUiFactory _uiFactory;

        public WindowService(IUiFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public async Task Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.Unknown:
                    break;
                case WindowId.MainMenu:
                    await _uiFactory.CreateMainMenu();
                    break;
                case WindowId.Settings:
                    break;
                case WindowId.EndGame:
                    _uiFactory.CreateEndGame();
                    break;
            }
        }
    }
}