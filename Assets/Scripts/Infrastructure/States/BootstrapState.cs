namespace Assets.Scripts.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string InitialScene = "InitialScene";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        //private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            //_services = services;
            //RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(InitialScene, onLoaded: EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

        //private void RegisterServices()
        //{
        //    RegisterStaticData();
        //    _services.RegisterSingle<IGameStateMachine>(_stateMachine);
        //    RegisterAssetProvider();
        //    _services.RegisterSingle<IRandomService>(new UnityRandomService());
        //    _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
        //    _services.RegisterSingle<IUIFactory>(new UiFactory(
        //        _services.Single<IAssets>(),
        //        _services.Single<IStaticDataService>(),
        //        _services.Single<IPersistentProgressService>()));
        //    _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));
        //    _services.RegisterSingle<IGameFactory>(new GameFactory(
        //        _services.Single<IAssets>(),
        //        _services.Single<IStaticDataService>(),
        //        _services.Single<IRandomService>(),
        //        _services.Single<IPersistentProgressService>(),
        //        _services.Single<IWindowService>()));
        //    _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(
        //        _services.Single<IPersistentProgressService>(), 
        //        _services.Single<IGameFactory>()));
        //}

        //private void RegisterAssetProvider()
        //{
        //    var assetProvider = new AssetProvider();
        //    assetProvider.Initialize();
        //    _services.RegisterSingle<IAssets>(assetProvider);
        //}

        public void Exit()
        {
            
        }

        //private void RegisterStaticData()
        //{
        //    var staticData = new StaticDataService();
        //    staticData.Load();
        //    _services.RegisterSingle<IStaticDataService>(staticData);
        //}
    }
}