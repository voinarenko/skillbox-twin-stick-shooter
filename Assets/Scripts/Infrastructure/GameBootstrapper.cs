using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.Logic;
using Assets.Scripts.UI.Services.Factory;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public LoadingCurtain CurtainPrefab;

        private Game _game;

        public void Construct(IStaticDataService staticData, IPersistentProgressService progressService, ISaveLoadService saveLoadService, IGameFactory gameFactory, IUIFactory uiFactory)
        {
            _game = new Game(this, Instantiate(CurtainPrefab), staticData, progressService, saveLoadService, gameFactory, uiFactory);
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}