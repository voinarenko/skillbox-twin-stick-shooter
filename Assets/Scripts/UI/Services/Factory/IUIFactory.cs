using System.Threading.Tasks;
using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.UI.Services.Factory
{
    public interface IUIFactory : IService
    {
        void CreateEndGame();
        Task CreateUIRoot();
    }
}