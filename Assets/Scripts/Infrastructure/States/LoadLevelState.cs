using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Infrastructure.Services.Wave;
using Assets.Scripts.Logic;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI.Services.Factory;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string WaveChangerTag = "WaveChanger";
        private static NetManager NetManager => Object.FindAnyObjectByType<NetManager>();
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        private readonly IStaticDataService _staticData;
        private readonly IUiFactory _uiFactory;
        private readonly IWaveService _waveService;
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, IPersistentProgressService progressService, IGameFactory gameFactory, LoadingCurtain loadingCurtain, IStaticDataService staticData, IUiFactory uiFactory, IWaveService waveService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _progressService = progressService;
            _gameFactory = gameFactory;
            _loadingCurtain = loadingCurtain;
            _staticData = staticData;
            _uiFactory = uiFactory;
            _waveService = waveService;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.CleanUp();
            _gameFactory.WarmUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private async void OnLoaded()
        {
            await InitUiRoot();
            InitGameWorld();

            _loadingCurtain.Hide();
            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitUiRoot() => 
            await _uiFactory.CreateUiRoot();

        private void InitGameWorld()
        {
            var levelData = LevelStaticData();
            NetManager.Construct(_progressService, _gameFactory, _waveService, levelData);
            InitWaveChanger();
        }

        private void InitWaveChanger()
        {
            var manager = GameObject.FindWithTag(WaveChangerTag);
                manager.GetComponent<WaveChanger>().Construct(_progressService, _stateMachine, _waveService);
                manager.GetComponent<PauseListener>().Construct(_stateMachine);
        }

        private LevelStaticData LevelStaticData() => 
            _staticData.ForLevel(SceneManager.GetActiveScene().name);
    }
}