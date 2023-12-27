using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Logic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadSceneState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";
        private const string CameraTag = "VirtualCamera";
        private const string EnemySpawnerTag = "SpawnPoint";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _loader;
        private readonly AllServices _services;
        private IPersistentProgressService _progressService;
        private IGameFactory _gameFactory;
        private LoadingCurtain _curtain;
        private IStaticDataService _staticData;

        public LoadSceneState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _loader = sceneLoader;
            _services = services;
        }

        public IExitableState Enter(string sceneToLoad)
        {
            _progressService = _services.Single<IPersistentProgressService>();
            _gameFactory = _services.Single<IGameFactory>();
            _staticData = _services.Single<IStaticDataService>();

            EnsureCurtain();
            _curtain.Show();

            _gameFactory.CleanUp();
            _loader.Load(sceneToLoad, OnLoaded);

            return this;
        }

        public void Exit() => 
            _curtain.Hide();

        private void EnsureCurtain()
        {
            if (_curtain == null) 
                _curtain = Resources.Load<LoadingCurtain>("Curtain");
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

        private void OnLoaded()
        {
            InitGameWorld();
            InformProgressReaders();

            _curtain.Hide();
            _stateMachine.Enter<GameLoopState>();
        }

        private void InformProgressReaders() => 
            _gameFactory.ProgressReaders.ForEach(x=>x.LoadProgress(_progressService.Progress));
    }
}