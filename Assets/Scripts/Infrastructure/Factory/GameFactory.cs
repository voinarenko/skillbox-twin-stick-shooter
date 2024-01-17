using Assets.Scripts.Enemy;
using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Randomizer;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Logic;
using Assets.Scripts.Logic.EnemySpawners;
using Assets.Scripts.Player;
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
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _progressService;

        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<ISavedProgress> ProgressWriters { get; } = new();

        private GameObject PlayerGameObject { get; set; }

        public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService)
        {
            _assets = assets;
            _staticData = staticData;
            _randomService = randomService;
            _progressService = progressService;
        }

        public async Task WarmUp()
        {
            await _assets.Load<GameObject>(AssetAddress.Loot);
            await _assets.Load<GameObject>(AssetAddress.Spawner);
        }

        public async Task<GameObject> CreatePlayer(Vector3 at)
        {
            //PlayerGameObject = await InstantiateRegisteredAsync(AssetAddress.PlayerPath, at);
            //return PlayerGameObject;

            var playerData = _progressService.Progress.PlayerStaticData;
            var prefab = await _assets.Load<GameObject>(playerData.PrefabReference);
            PlayerGameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(PlayerGameObject);
            //var health = PlayerGameObject.GetComponent<IHealth>();
            //health.Current = playerData.Health;
            //health.Max = playerData.Health;

            _progressService.Progress.WorldData.AmmoData.Available = playerData.Ammo;

            PlayerGameObject.GetComponent<PlayerMovement>().SetSpeed(playerData.MoveSpeed);
            PlayerGameObject.GetComponent<PlayerRotation>().SetSpeed(playerData.RotateSpeed);
            PlayerGameObject.GetComponent<PlayerShooter>().Construct(playerData, _progressService.Progress.WorldData, playerData.Damage, playerData.AttackCooldown, playerData.ReloadCooldown);
            return PlayerGameObject;
        }
        
        public async Task<GameObject> CreateHud()
        {
            var hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);
            
            hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);
            hud.GetComponentInChildren<AmmoCounter>().Construct(_progressService.Progress.WorldData);
            hud.GetComponent<ActorUi>().Construct(PlayerGameObject.GetComponent<IHealth>());

            return hud;
        }

        public async Task<GameObject> CreateEnemy(EnemyTypeId typeId, Transform parent)
        {
            var enemyData = _staticData.ForEnemy(typeId);

            var prefab = await _assets.Load<GameObject>(enemyData.PrefabReference);

            var enemy = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

            var health = enemy.GetComponent<IHealth>();
            health.Current = enemyData.Health;
            health.Max = enemyData.Health;

            enemy.GetComponent<ActorUi>().Construct(health);
            enemy.GetComponent<EnemyMoveToPlayer>().Construct(PlayerGameObject.transform);

            var agent = enemy.GetComponent<NavMeshAgent>();
            agent.speed = enemyData.MoveSpeed;
            agent.stoppingDistance = enemyData.StoppingDistance;
            agent.angularSpeed = enemyData.RotateSpeed;
            agent.acceleration = enemyData.Acceleration;

            var attack = enemy.GetComponent<EnemyAttack>();
            attack.Construct(PlayerGameObject.transform);
            attack.Type = (EnemyType)enemyData.EnemyTypeId;
            attack.Damage = enemyData.Damage;
            attack.Cleavage = enemyData.Cleavage;
            attack.AttackCooldown = enemyData.AttackCooldown;

            var lootSpawner = enemy.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _randomService);

            return enemy;
        }

        public async Task<LootPiece> CreateLoot()
        {
            var prefab = await _assets.Load<GameObject>(AssetAddress.Loot);
            var lootPiece = InstantiateRegistered(prefab)
                .GetComponent<LootPiece>();
            lootPiece.Construct(_progressService.Progress.WorldData);
            return lootPiece;
        }

        public async Task CreateSpawner(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId)
        {
            var prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);
            var spawner = InstantiateRegistered(prefab, at)
                .GetComponent<SpawnPoint>();
            spawner.Construct(this);
            spawner.Id = spawnerId;
            spawner.EnemyTypeId = enemyTypeId;
        }

        public void Register(ISavedProgressReader progressReader)
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

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
            _assets.CleanUp();
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

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            var gameObject = await _assets.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        //private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
        //{
        //    var gameObject = await _assets.Instantiate(prefabPath, at);
        //    RegisterProgressWatchers(gameObject);
        //    return gameObject;
        //}

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }
    }
}