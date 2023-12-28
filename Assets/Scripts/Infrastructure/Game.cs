using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.Logic;

namespace Assets.Scripts.Infrastructure
{
    internal class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain) => 
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain, AllServices.Container);
    }
}