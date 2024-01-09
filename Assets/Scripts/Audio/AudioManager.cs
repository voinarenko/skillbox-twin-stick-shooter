using Assets.Scripts.Infrastructure.Services.Audio;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private IAudioService _audioService;
        private ISaveLoadService _saveLoadService;

        public void Construct(IAudioService audioService, ISaveLoadService saveLoadService)
        {
            _audioService = audioService;
            _saveLoadService = saveLoadService;
            _audioService.InitVca();
            LoadSavedSettings();
            DontDestroyOnLoad(this);
        }

        private void LoadSavedSettings()
        {
            var settings = _saveLoadService.LoadSettings();
            if (settings == null) return;
            _audioService.SetVolume(settings);
        }
    }
}