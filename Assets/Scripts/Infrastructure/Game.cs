using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.States;

namespace Assets.Scripts.Infrastructure
{
    internal class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner) => 
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), AllServices.Container);
    }
}