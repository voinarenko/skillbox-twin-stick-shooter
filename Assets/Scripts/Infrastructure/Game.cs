using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Infrastructure.Services.Wave;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.Logic;
using Assets.Scripts.UI.Services.Factory;
using Assets.Scripts.UI.Services.Windows;

namespace Assets.Scripts.Infrastructure
{
    internal class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain, IStaticDataService staticData,
            IPersistentProgressService progressService, ISaveLoadService saveLoadService, IGameFactory gameFactory,
            IUiFactory uiFactory, IWindowService windowService, IWaveService waveService) =>
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain, staticData, progressService,
                saveLoadService, gameFactory, uiFactory, windowService, waveService);
    }
}