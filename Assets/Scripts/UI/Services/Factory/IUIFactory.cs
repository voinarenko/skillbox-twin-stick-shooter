using System.Threading.Tasks;
using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.UI.Services.Factory
{
    public interface IUiFactory : IService
    {
        void CreateEndGame();
        Task CreateUiRoot();
        Task CreateMainMenu();
    }
}