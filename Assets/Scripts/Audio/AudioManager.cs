using Assets.Scripts.Infrastructure.Services.Audio;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private IAudioService _audioService;
        private ISaveLoadService _saveLoadService;

        public void Construct(IAudioService audioService)
        {
            _audioService = audioService;
            DontDestroyOnLoad(this);
        }
    }
}