using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.UI.Elements.Buttons
{
    public interface IButtonService : IService
    {
        void StartGame();
        void OpenSettings();
        void QuitGame();
    }
}