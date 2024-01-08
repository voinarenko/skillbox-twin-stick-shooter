using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.UI.Services.Windows;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Services.Factory
{
    public class UiFactory : IUiFactory
    {
        private const string UiRootPath = "UIRoot";
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private Transform _uiRoot;

        public UiFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
        }

        public async Task CreateMainMenu()
        {
            var window = await _assets.Instantiate(AssetAddress.MainMenu);
            window.transform.SetParent(_uiRoot);
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