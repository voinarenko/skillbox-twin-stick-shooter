using System;
using System.Collections.Generic;
using Assets.Scripts.Logic.EnemySpawners;

namespace Assets.Scripts.Infrastructure.Services.Wave
{
    public interface IWaveService : IService
    {
        List<SpawnPoint> SpawnPoints { get; set; }
        void SpawnEnemies();
    }
}