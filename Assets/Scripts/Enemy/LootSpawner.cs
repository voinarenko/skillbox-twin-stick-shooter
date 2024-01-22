using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Randomizer;
using Assets.Scripts.StaticData;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        public  IPersistentProgressService ProgressService;
        public EnemyDeath EnemyDeath;
        private const float BoostFactor = 0.1f;
        private IGameFactory _factory;
        private IRandomService _random;

        public void Construct(IGameFactory factory, IRandomService random, IPersistentProgressService progress)
        {
            _factory = factory;
            _random = random;
            ProgressService = progress;
        }

        private void Start() => 
            EnemyDeath.Happened += SpawnLoot;

        private async void SpawnLoot()
        {
            if (!SpawnAllowed()) return;

            var loot = await _factory.CreateLoot();
            loot.transform.position = transform.position;

            var lootItem = GenerateLoot();
            loot.Initialize(lootItem);
        }

        private Loot GenerateLoot() =>
            new()
            {
                Type = (LootTypeId)_random.Next(0, (int)LootTypeId.Quantity)
            };

        private bool SpawnAllowed()
        {
            var target = (BoostFactor + BoostFactor * (ProgressService.Progress.WorldData.WaveData.Encountered - 1)) * 100;
            var random = _random.Next(0, 100);
            return random >= 0 && random <= target;
        }
    }
}