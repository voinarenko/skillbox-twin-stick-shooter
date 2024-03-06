using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.StaticData;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadProgressState : IPayloadedState<PlayerStaticData>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private PlayerStaticData _playerStaticData;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
        }

        public void Enter(PlayerStaticData payload)
        {
            _playerStaticData = payload;
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState, string>(PositionOnLevel.Level);
        }

        public void Exit() { }

        private void LoadProgressOrInitNew() => 
            _progressService.Progress = NewProgress();

        private PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress(_playerStaticData);
            progress.PlayerState.CurrentType = _playerStaticData.PlayerTypeId;
            progress.PlayerState.MaxHealth = _playerStaticData.Health;

            progress.PlayerState.ResetHealth();
            
            return progress;
        }
    }
}