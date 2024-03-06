using Assets.Scripts.Data;

namespace Assets.Scripts.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        void SaveSettings();
        Settings LoadSettings();
    }
}