using Assets.Scripts.Audio;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.Audio;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Infrastructure.Services.Wave;
using Assets.Scripts.UI.Services.Factory;
using Assets.Scripts.UI.Services.Windows;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        public GameBootstrapper BootstrapperPrefab;
        public AudioManager AudioManagerPrefab;
        private IStaticDataService _staticData;
        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        private IGameFactory _gameFactory;
        private IUiFactory _uiFactory;
        private IWindowService _windowService;
        private IAudioService _audioService;
        private IWaveService _waveService;

        [Inject]
        public void Construct(IStaticDataService staticData, IPersistentProgressService progressService,
            ISaveLoadService saveLoadService, IGameFactory gameFactory, IUiFactory uiFactory,
            IWindowService windowService, IAudioService audioService, IWaveService waveService)
        {
            _staticData = staticData;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _windowService = windowService;
            _audioService = audioService;
            _waveService = waveService;
        }

        private void Awake()
        {
            var bootstrapper = FindAnyObjectByType<GameBootstrapper>();
            if (!bootstrapper)
            {
                bootstrapper = Instantiate(BootstrapperPrefab);
                bootstrapper.Construct(_staticData, _progressService, _gameFactory, _uiFactory, _windowService, _waveService);
            }

            var audioManager = FindAnyObjectByType<AudioManager>();
            if (!audioManager)
            {
                audioManager = Instantiate(AudioManagerPrefab);
                audioManager.Construct(_audioService, _saveLoadService);
            }
        }
    }
}