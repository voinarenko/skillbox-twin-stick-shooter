namespace Assets.Scripts.Infrastructure.States
{
    public class MenuLoopState : IState
    {
        private readonly IGameStateMachine _stateMachine;

        public MenuLoopState(IGameStateMachine stateMachine) => 
            _stateMachine = stateMachine;

        public void Enter() { }

        public void Exit() { }
    }
}