using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Infrastructure.Services.Wave;
using Assets.Scripts.Logic;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI.Services.Factory;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string WaveChangerTag = "WaveChanger";
        private const string CameraTag = "VirtualCamera";
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
            await InitGameWorld();
            InformProgressReaders();

            _loadingCurtain.Hide();
            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitUiRoot() => 
            await _uiFactory.CreateUiRoot();

        private async Task InitGameWorld()
        {
            var levelData = LevelStaticData();
            await InitSpawners(levelData);
            //var player = await InitPlayer(levelData);

            NetManager.Construct(_gameFactory, _progressService.Progress.PlayerStaticData, levelData.InitialPlayerPosition);

            InitWaveChanger();
            //CameraFollow(player);

            //await InitHud();
 
            //NetManager.SpawnPlayer(player);
        }

        private void InitWaveChanger()
        {
            var manager = GameObject.FindWithTag(WaveChangerTag);
                manager.GetComponent<WaveChanger>().Construct(_progressService, _stateMachine, _waveService);
                manager.GetComponent<PauseListener>().Construct(_stateMachine);
        }

        private async Task InitSpawners(LevelStaticData levelData)
        {
            foreach (var spawnerData in levelData.EnemySpawners)
                await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id);
        }

        private void InformProgressReaders() => 
            _gameFactory.ProgressReaders.ForEach(x=>x.LoadProgress(_progressService.Progress));

        private async Task InitHud() => 
            await _gameFactory.CreateHud();

        private async Task<GameObject> InitPlayer(LevelStaticData levelData) => 
            await _gameFactory.CreatePlayer(levelData.InitialPlayerPosition);

        private LevelStaticData LevelStaticData() => 
            _staticData.ForLevel(SceneManager.GetActiveScene().name);

        private static void CameraFollow(GameObject player)
        {
            var camera = GameObject.FindWithTag(CameraTag).GetComponent<CinemachineVirtualCamera>();
            camera.Follow = player.transform;
            camera.LookAt = player.transform;
        }
    }
}