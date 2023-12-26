namespace Assets.Scripts.Infrastructure.States
{
    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface IPayloadedState<TPayload> : IExitableState
    {
        IExitableState Enter(TPayload payload);
    }

    public interface IExitableState
    {
        void Exit();
    }
}