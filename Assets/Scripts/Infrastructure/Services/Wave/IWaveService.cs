using Assets.Scripts.Logic.EnemySpawners;
using System.Collections.Generic;

namespace Assets.Scripts.Infrastructure.Services.Wave
{
    public interface IWaveService : IService
    {
        List<SpawnPoint> SpawnPoints { get; set; }
        void SpawnEnemies();
    }
}