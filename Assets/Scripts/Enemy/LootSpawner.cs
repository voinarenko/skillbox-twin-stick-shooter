using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Randomizer;
using Assets.Scripts.StaticData;
using Mirror;

namespace Assets.Scripts.Enemy
{
    public class LootSpawner : NetworkBehaviour
    {
        private IPersistentProgressService _progressService;
        public EnemyDeath EnemyDeath;
        private const float BoostFactor = 0.05f;
        private IGameFactory _factory;
        private IRandomService _random;

        public void Construct(IGameFactory factory, IRandomService random, IPersistentProgressService progress)
        {
            _factory = factory;
            _random = random;
            _progressService = progress;
        }

        private void Start() => 
            EnemyDeath.Happened += SpawnLoot;

        private async void SpawnLoot()
        {
            if (!SpawnAllowed()) return;

            var loot = await _factory.CreateLoot();
            NetworkServer.Spawn(loot.gameObject);
            loot.transform.position = transform.position;

            if (GenerateLoot() < 20)
            {
                var lootItem = GenerateConsumable();
                loot.Initialize(lootItem);
            }
            else
            {
                var lootItem = GeneratePerk();
                loot.Initialize(lootItem);
            }

        }

        private int GenerateLoot() => 
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
            var target = (BoostFactor + BoostFactor * (_progressService.Progress.WorldData.WaveData.Encountered - 1)) * 100;
            var random = _random.Next(0, 100);
            return random >= 0 && random <= target;
        }
    }
}