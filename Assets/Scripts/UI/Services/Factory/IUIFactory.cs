using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.States;
using System.Threading.Tasks;
using Assets.Scripts.UI.Services.Windows;

namespace Assets.Scripts.UI.Services.Factory
{
    public interface IUiFactory : IService
    {
        void CreateEndGame();
        Task CreateUiRoot();
        Task CreateMainMenu(IGameStateMachine stateMachine, IWindowService windowService);
        void CreateSettings();
    }
}