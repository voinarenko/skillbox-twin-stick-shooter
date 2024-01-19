using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.UI.Services.Factory;
using System.Threading.Tasks;

namespace Assets.Scripts.UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IUiFactory _uiFactory;

        public WindowService(IUiFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public async Task Open(WindowId windowId, IGameStateMachine stateMachine)
        {
            switch (windowId)
            {
                case WindowId.Unknown:
                    break;
                case WindowId.MainMenu:
                    await _uiFactory.CreateMainMenu(stateMachine, this);
                    break;
                case WindowId.Settings:
                    _uiFactory.CreateSettings();
                    break;
                case WindowId.EndGame:
                    _uiFactory.CreateEndGame();
                    break;
            }
        }
    }
}