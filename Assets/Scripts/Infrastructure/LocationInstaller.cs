using Assets.Scripts.Enemy;
using Assets.Scripts.Player;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Infrastructure
{
    public class LocationInstaller : MonoInstaller, IInitializable
    {
        public Transform StartPoint;
        public GameObject PlayerPrefab;
        public CinemachineVirtualCamera VirtualCamera;
        public EnemyMarker[] EnemyMarkers; 

        public override void InstallBindings()
        {
            BindInstallerInterfaces();
            BindPlayer();
            BindEnemyFactory();
        }

        private void BindEnemyFactory()
        {
            Container
                .Bind<IEnemyFactory>()
                .To<EnemyFactory>()
                .AsSingle();
        }

        private void BindInstallerInterfaces()
        {
            Container
                .BindInterfacesTo<LocationInstaller>()
                .FromInstance(this)
                .AsSingle();
        }

        private void BindPlayer()
        {
            var playerMovement = Container
                .InstantiatePrefabForComponent<PlayerMovement>(PlayerPrefab, StartPoint.position, Quaternion.identity, null);
            Container
                .Bind<PlayerMovement>()
                .FromInstance(playerMovement)
                .AsSingle()
                .NonLazy();
            VirtualCamera.Follow = playerMovement.transform;
            VirtualCamera.LookAt = playerMovement.transform;
        }

        public void Initialize()
        {
            var enemyFactory = Container.Resolve<IEnemyFactory>();
            foreach (var marker in EnemyMarkers)
            {
                enemyFactory.Load();
                enemyFactory.Create(marker.EnemyType, marker.transform.position);                
            }
        }
    }
}