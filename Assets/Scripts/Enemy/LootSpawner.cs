using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.Randomizer;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        public EnemyDeath EnemyDeath;
        private IGameFactory _factory;
        private IRandomService _random;

        public void Construct(IGameFactory factory, IRandomService random)
        {
            _factory = factory;
            _random = random;
        }

        private void Start() => 
            EnemyDeath.Happened += SpawnLoot;

        private async void SpawnLoot()
        {
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
    }
}