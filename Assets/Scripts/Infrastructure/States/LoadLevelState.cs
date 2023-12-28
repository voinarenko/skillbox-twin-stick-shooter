using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Logic;
using Assets.Scripts.UI.Services.Factory;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";
        private const string CameraTag = "VirtualCamera";
        private const string EnemySpawnerTag = "SpawnPoint";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IStaticDataService _staticData;
        private readonly IUIFactory _uiFactory;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, IPersistentProgressService progressService, IGameFactory gameFactory, LoadingCurtain loadingCurtain, IStaticDataService staticData, IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _progressService = progressService;
            _gameFactory = gameFactory;
            _loadingCurtain = loadingCurtain;
            _staticData = staticData;
            _uiFactory = uiFactory;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.CleanUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private void OnLoaded()
        {
            InitUIRoot();
            InitGameWorld();
            InformProgressReaders();

            _loadingCurtain.Hide();
            _stateMachine.Enter<GameLoopState>();
        }

        private void InitUIRoot()
        {
            _uiFactory.CreateUIRoot();
        }

        private void InitGameWorld()
        {
            InitSpawners();

            var player = InitPlayer();
            CameraFollow(player);

            InitHud();
        }

        private void InitSpawners()
        {
            var sceneKey = SceneManager.GetActiveScene().name;
            var levelData = _staticData.ForLevel(sceneKey);
            foreach (var spawnerData in levelData.EnemySpawners)
            {
                _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.EnemyTypeId);
            }
        }

        private static void CameraFollow(GameObject player)
        {
            var camera = GameObject.FindWithTag(CameraTag).GetComponent<CinemachineVirtualCamera>();
            camera.Follow = player.transform;
            camera.LookAt = player.transform;
        }

        private void InitHud() => 
            _gameFactory.CreateHud();

        private GameObject InitPlayer() => 
            _gameFactory.CreatePlayer(GameObject.FindWithTag(InitialPointTag));

        private void InformProgressReaders() => 
            _gameFactory.ProgressReaders.ForEach(x=>x.LoadProgress(_progressService.Progress));
    }
}