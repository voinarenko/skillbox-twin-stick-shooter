using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.UI.Elements.Buttons;
using Zenject;

namespace Assets.Scripts.Infrastructure.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
            Container.Bind<PlayButton>().AsSingle();
        }
    }
}