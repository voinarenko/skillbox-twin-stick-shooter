namespace Assets.Scripts.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string InitialScene = "InitialScene";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter() => 
            _sceneLoader.Load(InitialScene, onLoaded: EnterMainMenu);

        public void Exit() { }

        private void EnterMainMenu() => 
            _stateMachine.Enter<LoadMenuState>();
    }
}