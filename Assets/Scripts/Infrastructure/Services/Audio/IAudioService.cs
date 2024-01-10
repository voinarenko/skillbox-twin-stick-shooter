using Assets.Scripts.Data;
using UnityEngine.UI;

namespace Assets.Scripts.Infrastructure.Services.Audio
{
    public interface IAudioService : IService
    {
        void InitVca();
        void GetVolume();
        void SetVolume(Settings settings);
        void StoreVolume(Settings settings);
        void CancelChanges();
        void UpdateSliders(Slider masterSlider, Slider musicSlider, Slider effectsSlider);
    }
}