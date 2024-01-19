using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Logic.EnemySpawners;
using System.Collections.Generic;

namespace Assets.Scripts.Infrastructure.Services.Wave
{
    public class WaveService : IWaveService
    {
        public List<SpawnPoint> SpawnPoints { get; set; }

        private readonly IPersistentProgressService _progressService;
        
        public WaveService(IPersistentProgressService progressService) => 
            _progressService = progressService;

        public void SpawnEnemies()
        {
            for (var i = 0; i < _progressService.Progress.WorldData.WaveData.Encountered; i++)
                foreach (var point in SpawnPoints) 
                    point.Spawn();
        }
    }
}