using Assets.Scripts.Data;

namespace Assets.Scripts.Infrastructure.Services.Audio
{
    public interface IAudioService : IService
    {
        void InitVca();
        void GetVolume();
        void SetVolume(Settings settings);
        void StoreVolume(Settings settings);
    }
}