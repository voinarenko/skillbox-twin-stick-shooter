using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.StaticData;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadProgressState : IPayloadedState<PlayerStaticData>
    {
        private const string InitialLevel = "MainScene";
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private PlayerStaticData _playerStaticData;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter(PlayerStaticData payload)
        {
            _playerStaticData = payload;
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
        }

        public void Exit() { }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress =
                _saveLoadService.LoadProgress()
                ?? NewProgress();
        }

        private PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress(InitialLevel, _playerStaticData);
            progress.PlayerState.CurrentType = _playerStaticData.PlayerTypeId;
            progress.PlayerState.MaxHealth = _playerStaticData.Health;
            progress.PlayerStats.Damage = _playerStaticData.Damage;

            progress.PlayerState.ResetHealth();
            
            return progress;
        }
    }
}