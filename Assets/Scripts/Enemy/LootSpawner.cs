using System.Threading.Tasks;
using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Randomizer;
using Assets.Scripts.StaticData;
using Mirror;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class LootSpawner : NetworkBehaviour
    {
        private const float BoostFactor = 0.05f;
        [SerializeField] private EnemyDeath _enemyDeath;
        private IPersistentProgressService _progressService;
        private IGameFactory _factory;
        private IRandomService _random;

        public void Construct(IGameFactory factory, IRandomService random, IPersistentProgressService progress)
        {
            _factory = factory;
            _random = random;
            _progressService = progress;
        }

        private void Start() =>
            _enemyDeath.Happened += CmdSpawnLoot;

        private void OnDestroy() =>
            _enemyDeath.Happened -= CmdSpawnLoot;

        [Command(requiresAuthority = false)]
        private async void CmdSpawnLoot()
        {
            if (!isServer) return;
            if (!SpawnAllowed()) return;

            var loot = await _factory.CreateLoot();
            loot.transform.position = transform.position;
            await SelectLootType(loot);
            NetworkServer.Spawn(loot.gameObject);
            loot.RpcRefreshMaterial();
       }

        [Server]
        private Task SelectLootType(LootPiece loot)
        {
            if (GetLootZone() < 20)
            {
                var lootItem = GenerateConsumable();
                loot.Initialize(lootItem);
            }
            else
            {
                var lootItem = GeneratePerk();
                loot.Initialize(lootItem);
            }

            return Task.CompletedTask;
        }

        private int GetLootZone() => 
            _random.Next(0, 100);

        private Consumable GenerateConsumable() =>
            new()
            {
                Type = (ConsumableTypeId)_random.Next(0, (int)ConsumableTypeId.Quantity)
            };

        private Perk GeneratePerk() =>
            new()
            {
                Type = (PerkTypeId)_random.Next(0, (int)PerkTypeId.Quantity)
            };

        private bool SpawnAllowed()
        {
            return true;
            //var target = (BoostFactor + BoostFactor * (_progressService.Progress.WorldData.WaveData.Encountered - 1)) * 100;
            //var random = _random.Next(0, 100);
            //return random >= 0 && random <= target;
        }
    }
}