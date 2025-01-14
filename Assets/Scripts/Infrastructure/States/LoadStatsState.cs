﻿using Assets.Scripts.Logic;
using Assets.Scripts.UI.Services.Factory;
using Assets.Scripts.UI.Services.Windows;
using System.Threading.Tasks;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadStatsState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IUiFactory _uiFactory;
        private readonly IWindowService _windowService;
        private readonly LoadingCurtain _loadingCurtain;

        public LoadStatsState(IGameStateMachine stateMachine, IUiFactory uiFactory, LoadingCurtain loadingCurtain, IWindowService windowService)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            _loadingCurtain = loadingCurtain;
            _windowService = windowService;
        }

        public async void Enter()
        {
            _loadingCurtain.Show();
            await InitUiRoot();
            await _windowService.Open(WindowId.EndGame, _stateMachine);
            _stateMachine.Enter<StatsLoopState>();
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private async Task InitUiRoot() => 
            await _uiFactory.CreateUiRoot();
    }
}