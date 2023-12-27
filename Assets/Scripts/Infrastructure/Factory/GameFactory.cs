using System.Collections.Generic;
using Assets.Scripts.Enemy;
using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Randomizer;
using Assets.Scripts.Logic;
using Assets.Scripts.Logic.EnemySpawners;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI;
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
        public GameObject CreatePlayer(GameObject at)
        {
            PlayerGameObject = InstantiateRegistered(AssetPath.PlayerPath, at.transform.position);
            return PlayerGameObject;
        }


        public GameObject CreateHud()
        {
            var hud = InstantiateRegistered(AssetPath.HudPath);
            
            hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);
            
            return hud;
        }

        public GameObject CreateEnemy(EnemyTypeId typeId, Transform parent)
        {
            var enemyData = _staticData.ForEnemy(typeId);
            var enemy = Object.Instantiate(enemyData.Prefab, parent.position, Quaternion.identity, parent);

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

            var attack = enemy.GetComponent<Attack>();
            attack.Construct(PlayerGameObject.transform);
            attack.Damage = enemyData.Damage;
            attack.Cleavage = enemyData.Cleavage;
            attack.EffectiveDistance = enemyData.EffectiveDistance;
            attack.AttackCooldown = enemyData.AttackCooldown;

            var lootSpawner = enemy.GetComponentInChildren<LootSpawner>();
            lootSpawner.SetLoot(enemyData.MinLoot, enemyData.MaxLoot);
            lootSpawner.Construct(this, _randomService);

            return enemy;
        }

        public LootPiece CreateLoot()
        {
            var lootPiece = InstantiateRegistered(AssetPath.Loot).GetComponent<LootPiece>();
            lootPiece.Construct(_progressService.Progress.WorldData);
            return lootPiece;
        }

        public void CreateSpawner(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId)
        {
            var spawner = InstantiateRegistered(AssetPath.Spawner, at)
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
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            var gameObject = _assets.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            var gameObject = _assets.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }
    }
}