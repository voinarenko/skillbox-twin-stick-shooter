using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Wave;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.Player;
using Assets.Scripts.UI.Services.Windows;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class WaveChanger : MonoBehaviour
    {
        private const string FinalScene = "FinalScene";
        private IPersistentProgressService _progressService;
        private IGameStateMachine _stateMachine;
        private IWaveService _waveService;
        private SceneLoader _sceneLoader;
        
        private PlayerDeath _playerDeath;

        private WaveData _waveData;

        public void Construct(IPersistentProgressService progressService, IGameStateMachine stateMachine, IWaveService waveService, PlayerDeath playerDeath)
        {
            _progressService = progressService;
            _stateMachine = stateMachine;
            _waveService = waveService;
            _waveData = _progressService.Progress.WorldData.WaveData;
            _playerDeath = playerDeath;
            _waveData.EnemyRemoved += CheckEnemies;
            _playerDeath.Happened += GameOver;
        }

        public void Init(SceneLoader sceneLoader) => 
            _sceneLoader = sceneLoader;

        private void GameOver() => 
            _sceneLoader.Load(FinalScene, onLoaded: EnterStats);

        private void EnterStats() =>
            _stateMachine.Enter<LoadStatsState>();

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