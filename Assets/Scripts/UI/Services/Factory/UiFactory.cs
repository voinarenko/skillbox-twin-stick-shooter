using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.UI.Services.Windows;
using Assets.Scripts.UI.Windows;
using System.Threading.Tasks;
using Assets.Scripts.Infrastructure.Services.Audio;
using Assets.Scripts.Infrastructure.Services.Parameters;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace Assets.Scripts.UI.Services.Factory
{
    public class UiFactory : IUiFactory
    {
        private const string UiRootPath = "UIRoot";
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAudioService _audioService;
        private readonly ISettingsService _settingsService;
        private Transform _uiRoot;

        public UiFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService progressService, ISaveLoadService saveLoadService, IAudioService audioService, ISettingsService settingsService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _audioService = audioService;
            _settingsService = settingsService;
        }

        public async Task CreateMainMenu(IGameStateMachine stateMachine)
        {
            var window = await _assets.Instantiate(AssetAddress.MainMenu);
            window.transform.SetParent(_uiRoot);
            var baseWindow = window.GetComponent<BaseWindow>();
            baseWindow.Construct(_audioService);
            baseWindow.Init();
            var buttons = window.GetComponent<MenuWindow>();
            buttons.PlayButton.Construct(stateMachine);
            buttons.SettingsButton.Construct(this);
            
        }

        public void CreateSettings()
        {
            var config = _staticData.ForWindow(WindowId.Settings);
            var window = Object.Instantiate(config.Prefab, _uiRoot);
            window.Construct(_saveLoadService, _audioService, _settingsService);
            window.Init();
        }

        public void CreateEndGame()
        {
            var config = _staticData.ForWindow(WindowId.EndGame);
            var window = Object.Instantiate(config.Prefab, _uiRoot);
            window.Construct(_progressService);
        }

        public async Task CreateUiRoot()
        {
            var root = await _assets.Instantiate(UiRootPath);
            _uiRoot = root.transform;
        }
    }
}