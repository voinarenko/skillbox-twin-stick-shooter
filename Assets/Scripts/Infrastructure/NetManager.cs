using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.StaticData;
using Cinemachine;
using Mirror;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class NetManager : NetworkManager
    {
        private IGameFactory _gameFactory;

        private PlayerStaticData _playerData;
        [SerializeField] private GameObject[] _playerPrefabs;

        private Vector3 _spawnPosition;
        
        private bool _playerSpawned;
        private bool _playerConnected;

        public void Construct(IGameFactory gameFactory, PlayerStaticData playerData, Vector3 position)
        {
            _gameFactory = gameFactory;
            _playerData = playerData;
            _spawnPosition = position;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreateCharacter);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            _playerConnected = true;

            if (!_playerSpawned && _playerConnected)
                ActivatePlayerSpawn();
        }

        private void ActivatePlayerSpawn()
        {
            var playerType = (int)_playerData.PlayerTypeId;
            var message = new CreatePlayerMessage
            {
               PlayerType = playerType
            };
            NetworkClient.Send(message);
            _playerSpawned = true;
        }

        private void OnCreateCharacter(NetworkConnectionToClient conn, CreatePlayerMessage message)
        {
            var player = Instantiate(_playerPrefabs[message.PlayerType], _spawnPosition, Quaternion.identity);
            _gameFactory.UpdatePlayerData(player);
            NetworkServer.AddPlayerForConnection(conn, player);

            var virtualCameras = GameObject.FindGameObjectsWithTag("VirtualCamera");
            foreach ( var virtualCam in virtualCameras)
            {
                var virtualCamera = virtualCam.GetComponent<CinemachineVirtualCamera>();
                if (virtualCamera.Follow != null) return;
                virtualCamera.Follow = player.transform;
                virtualCamera.LookAt = player.transform;
            }
        }
    }
}