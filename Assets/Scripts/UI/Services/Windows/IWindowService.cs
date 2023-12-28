using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.UI.Services.Windows
{
    public interface IWindowService : IService
    {
        void Open(WindowId windowId);
    }
}