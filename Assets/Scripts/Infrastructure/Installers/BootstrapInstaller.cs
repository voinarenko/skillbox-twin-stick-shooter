using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.Audio;
using Assets.Scripts.Infrastructure.Services.Parameters;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Randomizer;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Infrastructure.Services.Wave;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.UI.Services.Factory;
using Assets.Scripts.UI.Services.Windows;
using Zenject;

namespace Assets.Scripts.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStaticData();
            BindGameStateMachine();
            BindAssetsProvider();
            BindRandomService();
            BindProgressService();
            BindUiFactory();
            BindWindowService();
            BindGameFactory();
            BindSaveLoadService();

            BindBootstrapper();
            BindLoadProgressState();
            BindAudioService();
            BindSettingsService();
            BindWaveService();
        }

        private void BindStaticData()
        {
            var staticData = new StaticDataService();
            staticData.Load();
            Container
                .Bind<IStaticDataService>()
                .FromInstance(staticData)
                .AsSingle();
        }

        private void BindGameStateMachine() => 
            Container
                .Bind<IGameStateMachine>()
                .To<GameStateMachine>()
                .AsSingle();

        private void BindAssetsProvider()
        {
            var assetProvider = new AssetProvider();
            assetProvider.Initialize();
            Container
                .Bind<IAssets>()
                .FromInstance(assetProvider)
                .AsSingle();
        }

        private void BindRandomService() => 
            Container
                .Bind<IRandomService>()
                .To<UnityRandomService>()
                .AsSingle();

        private void BindProgressService() =>
            Container
                .Bind<IPersistentProgressService>()
                .To<PersistentProgressService>()
                .AsSingle();

        private void BindUiFactory() =>
            Container
                .Bind<IUiFactory>()
                .To<UiFactory>()
                .AsSingle();

        private void BindWindowService() =>
            Container
                .Bind<IWindowService>()
                .To<WindowService>()
                .AsSingle();

        private void BindGameFactory() =>
            Container
                .Bind<IGameFactory>()
                .To<GameFactory>()
                .AsSingle()
                .NonLazy();

        private void BindSaveLoadService() =>
            Container
                .Bind<ISaveLoadService>()
                .To<SaveLoadService>()
                .AsSingle();

        private void BindBootstrapper() => 
            Container
                .Bind<GameBootstrapper>()
                .AsSingle();

        private void BindLoadProgressState() => 
            Container
                .Bind<LoadProgressState>()
                .AsSingle();

        private void BindAudioService() => 
            Container
                .Bind<IAudioService>()
                .To<AudioService>()
                .AsSingle();

        private void BindSettingsService() => 
            Container
                .Bind<ISettingsService>()
                .To<SettingsService>()
                .AsSingle();

        private void BindWaveService() => 
            Container
                .Bind<IWaveService>()
                .To<WaveService>()
                .AsSingle();
    }
}