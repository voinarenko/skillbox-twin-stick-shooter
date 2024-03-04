using Assets.Scripts.Enemy;
using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Services.Loot;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Randomizer;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Infrastructure.Services.Wave;
using Assets.Scripts.Logic;
using Assets.Scripts.Logic.EnemySpawners;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI.Elements;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<ISavedProgress> ProgressWriters { get; } = new();
        
        private const string WaveChangerTag = "WaveChanger";
        private static PlayersWatcher PlayersWatcher => GameObject.FindWithTag(WaveChangerTag).GetComponent<PlayersWatcher>();

        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _progressService;
        private readonly IWaveService _waveService;
        private readonly ILootService _lootService;

        public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService, IWaveService waveService, ILootService lootService)
        {
            _assets = assets;
            _staticData = staticData;
            _randomService = randomService;
            _progressService = progressService;
            _waveService = waveService;
            _lootService = lootService;
        }

        public async Task WarmUp()
        {
            await _assets.Load<GameObject>(AssetAddress.Loot);
            await _assets.Load<GameObject>(AssetAddress.Spawner);
        }

        public async Task<GameObject> CreateEnemy(EnemyTypeId typeId, Transform parent)
        {
            var enemyData = _staticData.ForEnemy(typeId);

            var prefab = await _assets.Load<GameObject>(enemyData.PrefabReference);

            var enemy = Object.Instantiate(prefab, parent.position, Quaternion.identity);

            var health = enemy.GetComponent<IHealth>();
            health.Current = enemyData.Health + enemyData.Health * enemyData.BoostFactor *
                (_progressService.Progress.WorldData.WaveData.Encountered - 1);
            health.Max = enemyData.Health;

            enemy.GetComponent<ActorUi>().Construct(health);

            var agent = enemy.GetComponent<NavMeshAgent>();
            agent.speed = enemyData.MoveSpeed;
            agent.stoppingDistance = enemyData.StoppingDistance;
            agent.angularSpeed = enemyData.RotateSpeed;
            agent.acceleration = enemyData.Acceleration;

            var attack = enemy.GetComponent<EnemyAttack>();
            attack.Type = (EnemyType)enemyData.EnemyTypeId;
            attack.Damage = enemyData.Damage + enemyData.Damage * enemyData.BoostFactor *
                (_progressService.Progress.WorldData.WaveData.Encountered - 1);
            attack.Cleavage = enemyData.Cleavage;
            attack.AttackCooldown = enemyData.AttackCooldown;

            var death = enemy.GetComponent<EnemyDeath>();
            death.Construct(_progressService.Progress, PlayersWatcher);
            death.Value = enemyData.KillValue;

            var lootSpawner = enemy.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _randomService, _progressService);

            return enemy;
        }

        public async Task<LootPiece> CreateLoot()
        {
            var prefab = await _assets.Load<GameObject>(AssetAddress.Loot);
            var lootPiece = InstantiateRegistered(prefab).GetComponent<LootPiece>();
            lootPiece.Construct(_lootService);
            return lootPiece;
        }

        public async Task<GameObject> CreateSpawner(Vector3 at, string spawnerId)
        {
            var prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);
            var spawner = InstantiateRegistered(prefab, at).GetComponent<SpawnPoint>();
            spawner.Construct(this, _randomService, _progressService);
            spawner.Id = spawnerId;
            _waveService.SpawnPoints ??= new List<SpawnPoint>();
            _waveService.SpawnPoints.Add(spawner);

            return spawner.gameObject;
        }

        public async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            var gameObject = await _assets.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
            _assets.CleanUp();
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressPiece)
            {
                if (!ProgressWriters.Contains(progressPiece)) 
                    ProgressWriters.Add(progressPiece);
            }

            if (!ProgressReaders.Contains(progressReader))
            {
                ProgressReaders.Add(progressReader);
            }
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            var gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            var gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        public void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }
    }
}