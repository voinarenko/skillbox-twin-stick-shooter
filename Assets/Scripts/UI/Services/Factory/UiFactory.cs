using System.Threading.Tasks;
using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.UI.Services.Windows;
using UnityEngine;

namespace Assets.Scripts.UI.Services.Factory
{
    public class UiFactory : IUIFactory
    {
        private const string UIRootPath = "UIRoot";
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

        public void CreateEndGame()
        {
            var config = _staticData.ForWindow(WindowId.EndGame);
            var window = Object.Instantiate(config.Prefab, _uiRoot);
            window.Construct(_progressService);
        }

        public async Task CreateUIRoot()
        {
            var root = await _assets.Instantiate(UIRootPath);
            _uiRoot = root.transform;
        }
    }
}