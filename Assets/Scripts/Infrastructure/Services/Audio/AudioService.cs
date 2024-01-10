using Assets.Scripts.Data;
using UnityEngine.UI;

namespace Assets.Scripts.Infrastructure.Services.Audio
{
    public class AudioService : IAudioService
    {
        private const string MasterVcaName = "Master";
        private const string MusicVcaName = "Music";
        private const string EffectsVcaName = "Effects";

        private FMOD.Studio.VCA _masterVca;
        private FMOD.Studio.VCA _musicVca;
        private FMOD.Studio.VCA _effectsVca;

        private float _masterVolume;
        private float _musicVolume;
        private float _effectsVolume;

        public void InitVca()
        {
            _masterVca = FMODUnity.RuntimeManager.GetVCA($"vca:/{MasterVcaName}");
            _musicVca = FMODUnity.RuntimeManager.GetVCA($"vca:/{MusicVcaName}");
            _effectsVca = FMODUnity.RuntimeManager.GetVCA($"vca:/{EffectsVcaName}");
        }

        public void SetVolume(Settings settings)
        {
            _masterVca.setVolume(settings.Volume.MasterVolume);
            _musicVca.setVolume(settings.Volume.MusicVolume);
            _effectsVca.setVolume(settings.Volume.EffectsVolume);
        }

        public void GetVolume()
        {
            _masterVca.getVolume(out _masterVolume);
            _musicVca.getVolume(out _musicVolume);
            _effectsVca.getVolume(out _effectsVolume);
        }

        public void StoreVolume(Settings settings)
        {
            _masterVca.getVolume(out var masterVolume);
            _musicVca.getVolume(out var musicVolume);
            _effectsVca.getVolume(out var effectsVolume);
            settings.Volume.MasterVolume = masterVolume;
            settings.Volume.MusicVolume = musicVolume;
            settings.Volume.EffectsVolume = effectsVolume;
        }

        public void CancelChanges()
        {
            _masterVca.setVolume(_masterVolume);
            _musicVca.setVolume(_musicVolume);
            _effectsVca.setVolume(_effectsVolume);
        }

        public void UpdateSliders(Slider masterSlider, Slider musicSlider, Slider effectsSlider)
        {
            masterSlider.value = _masterVolume;
            musicSlider.value = _musicVolume;
            effectsSlider.value = _effectsVolume;
        }
    }
}