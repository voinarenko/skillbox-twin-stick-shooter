using Cinemachine;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Common.Infrastructure
{
    public class LocationInstaller : MonoInstaller
    {
        public Transform StartPoint;
        public GameObject PlayerPrefab;
        public CinemachineVirtualCamera VirtualCamera;

        public override void InstallBindings()
        {
            BindPlayer();
        }

        private void BindPlayer()
        {
            var playerMovement = Container
                .InstantiatePrefabForComponent<PlayerMovement>(PlayerPrefab, StartPoint.position, Quaternion.identity, null);
            Container
                .Bind<PlayerMovement>()
                .FromInstance(playerMovement)
                .AsSingle();
            VirtualCamera.Follow = playerMovement.transform;
            VirtualCamera.LookAt = playerMovement.transform;
        }
    }
}