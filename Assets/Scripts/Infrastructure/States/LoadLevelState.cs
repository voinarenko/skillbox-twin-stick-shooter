using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Logic;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI.Services.Factory;
using Cinemachine;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string CameraTag = "VirtualCamera";
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        private readonly IStaticDataService _staticData;
        private readonly IUIFactory _uiFactory;
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;

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
            _gameFactory.WarmUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private async void OnLoaded()
        {
            await InitUIRoot();
            await InitGameWorld();
            InformProgressReaders();

            _loadingCurtain.Hide();
            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitUIRoot() => 
            await _uiFactory.CreateUIRoot();

        private async Task InitGameWorld()
        {
            var levelData = LevelStaticData();
            await InitSpawners(levelData);
            //await InitDroppedLoot();
            var player = await InitPlayer(levelData);
            CameraFollow(player);

            await InitHud();
        }

        private async Task InitSpawners(LevelStaticData levelData)
        {
            foreach (var spawnerData in levelData.EnemySpawners)
                await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.EnemyTypeId);
        }

        private void InformProgressReaders() => 
            _gameFactory.ProgressReaders.ForEach(x=>x.LoadProgress(_progressService.Progress));

        private async Task InitHud() => 
            await _gameFactory.CreateHud();

        //private async Task InitDroppedLoot()
        //{
        //    var droppedLoot = _progressService.Progress.WorldData.DroppedLoot;
        //    foreach (var drop in droppedLoot.Items)
        //    {
        //        var lootPiece = await _gameFactory.CreateLoot();
        //        lootPiece.transform.position = drop.Position.AsUnityVector();
        //        lootPiece.Initialize(drop.Loot);
        //    }

        //}

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