using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Wave;
using Assets.Scripts.Player;
using Assets.Scripts.UI.Services.Windows;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class WaveChanger : MonoBehaviour
    {
        private IPersistentProgressService _progressService;
        private IWaveService _waveService;
        private PlayerDeath _playerDeath;

        private WaveData _waveData;

        public void Construct(IPersistentProgressService progressService, IWaveService waveService, PlayerDeath playerDeath)
        {
            _progressService = progressService;
            _waveService = waveService;
            _waveData = _progressService.Progress.WorldData.WaveData;
            _playerDeath = playerDeath;
            _waveData.EnemyRemoved += CheckEnemies;
            _playerDeath.Happened += GameOver;
        }

        private void GameOver()
        {
            //_windowService.Open(WindowId.EndGame);
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