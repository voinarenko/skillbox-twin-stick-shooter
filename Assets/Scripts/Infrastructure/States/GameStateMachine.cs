﻿using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Logic;
using Assets.Scripts.UI.Services.Factory;
using Assets.Scripts.UI.Services.Windows;
using System;
using System.Collections.Generic;
using Assets.Scripts.Infrastructure.Services.Wave;

namespace Assets.Scripts.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IStaticDataService staticData,
            IPersistentProgressService progressService, ISaveLoadService saveLoadService, IGameFactory gameFactory,
            IUiFactory uiFactory, IWindowService windowService, IWaveService waveService)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(LoadMenuState)] = new LoadMenuState(this, uiFactory, loadingCurtain, windowService),
                [typeof(MenuLoopState)] = new MenuLoopState(),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, progressService, gameFactory, loadingCurtain, staticData, uiFactory, waveService),
                [typeof(LoadProgressState)] = new LoadProgressState(this, progressService, saveLoadService),
                [typeof(GameLoopState)] = new GameLoopState(sceneLoader, windowService, waveService),
                [typeof(LoadStatsState)] = new LoadStatsState(this, uiFactory, loadingCurtain, windowService),
                [typeof(StatsLoopState)] = new StatsLoopState()
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>  
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            var state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;
    }
}