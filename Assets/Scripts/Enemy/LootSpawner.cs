using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Randomizer;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        public EnemyDeath EnemyDeath;
        [SerializeField] private float _boostFactor = 0.1f;
        private IGameFactory _factory;
        private IRandomService _random;
        private IPersistentProgressService _progressService;

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
            loot.transform.position = transform.position;

            var lootItem = GenerateLoot();
            loot.Initialize(lootItem);
        }

        private Loot GenerateLoot() =>
            new()
            {
                Type = (LootType)_random.Next(0, (int)LootType.Quantity)
            };

        private bool SpawnAllowed()
        {
            var target = (_boostFactor + _boostFactor * (_progressService.Progress.WorldData.WaveData.Encountered - 1)) * 100;
            var random = _random.Next(0, 101);
            return random >= 0 && random <= target;
        }
    }
}