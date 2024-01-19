using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Wave;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class WaveChanger : MonoBehaviour
    {
        private IPersistentProgressService _progressService;
        private IWaveService _waveService;

        private WaveData _waveData;

        public void Construct(IPersistentProgressService progressService, IWaveService waveService)
        {
            _progressService = progressService;
            _waveService = waveService;
            _waveData = _progressService.Progress.WorldData.WaveData;
            _waveData.EnemyRemoved += CheckEnemies;
        }

        private void CheckEnemies()
        {
            if (_waveData.GetEnemies() <= 0)
            {
                _waveData.NextWave();
                _waveService.SpawnEnemies();
            }
        }
    }
}