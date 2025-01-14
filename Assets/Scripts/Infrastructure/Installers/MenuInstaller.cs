﻿using Assets.Scripts.Infrastructure.States;
using Zenject;

namespace Assets.Scripts.Infrastructure.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
            Container.Bind<LoadMenuState>().AsSingle();
        }
    }
}