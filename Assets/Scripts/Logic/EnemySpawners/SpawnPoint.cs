using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Randomizer;
using Assets.Scripts.StaticData;
using UnityEngine;

namespace Assets.Scripts.Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public string Id { get; set; }

        private IGameFactory _factory;
        private IRandomService _random;
        private IPersistentProgressService _progress;

        public void Construct(IGameFactory factory, IRandomService random, IPersistentProgressService progress)
        {
            _factory = factory;
            _random = random;
            _progress = progress;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            //Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            //_progress = progress;
        }

        public async void Spawn()
        {
            await _factory.CreateEnemy(GenerateRandomEnemy(), transform);
            _progress.Progress.WorldData.WaveData.AddEnemy();
        }

        private EnemyTypeId GenerateRandomEnemy()
        {
            var result = _random.Next(0, 100) switch
            {
                >= 0 and < 50 => EnemyTypeId.SmallMelee,
                >= 50 and < 80 => EnemyTypeId.BigMelee,
                >= 80 and < 100 => EnemyTypeId.Ranged,
                _ => EnemyTypeId.SmallMelee
            };
            return result;
        }
    }
}