using System.Threading.Tasks;
using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.UI.Services.Windows
{
    public interface IWindowService : IService
    {
        Task Open(WindowId windowId);
    }
}