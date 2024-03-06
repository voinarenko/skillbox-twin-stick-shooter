using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Wave;
using Assets.Scripts.Infrastructure.States;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class WaveChanger : MonoBehaviour
    {
        private const string FinalSceneName = "FinalScene";

        private IGameStateMachine _stateMachine;
        private IWaveService _waveService;
        
        private SceneLoader _sceneLoader;
        private WaveData _waveData;

        public void Construct(IPersistentProgressService progressService, IGameStateMachine stateMachine, IWaveService waveService)
        {
            _stateMachine = stateMachine;
            _waveService = waveService;
            _waveData = progressService.Progress.WorldData.WaveData;
            _waveData.EnemyRemoved += CheckEnemies;
            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            if (_waveData != null) 
                _waveData.EnemyRemoved -= CheckEnemies;
        }

        public void Init(SceneLoader sceneLoader) => 
            _sceneLoader = sceneLoader;

        public void GameOver() => 
            _sceneLoader.Load(FinalSceneName, onLoaded: EnterStats);

        private void EnterStats() =>
            _stateMachine.Enter<LoadStatsState>();

        private void CheckEnemies()
        {
            if (_waveData.GetEnemies() > 0) return;
            _waveData.NextWave();
            _waveService.SpawnEnemies();
        }
    }
}