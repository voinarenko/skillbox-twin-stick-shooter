using Assets.Scripts.Infrastructure.Services;
using System.Threading.Tasks;

namespace Assets.Scripts.UI.Services.Windows
{
    public interface IWindowService : IService
    {
        Task Open(WindowId windowId);
    }
}