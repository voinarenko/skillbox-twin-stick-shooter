using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.UI.Services.Factory;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        public GameBootstrapper BootstrapperPrefab;
        private IStaticDataService _staticData;
        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        private IGameFactory _gameFactory;
        private IUiFactory _uiFactory;

        [Inject]
        public void Construct(IStaticDataService staticData, IPersistentProgressService progressService, ISaveLoadService saveLoadService, IGameFactory gameFactory, IUiFactory uiFactory)
        {
            _staticData = staticData;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
        }

        private void Awake()
        {
            var bootstrapper = FindAnyObjectByType<GameBootstrapper>();
            if (!bootstrapper)
            {
                bootstrapper = Instantiate(BootstrapperPrefab);
                bootstrapper.Construct(_staticData, _progressService, _saveLoadService, _gameFactory, _uiFactory);
            }
        }
    }
}