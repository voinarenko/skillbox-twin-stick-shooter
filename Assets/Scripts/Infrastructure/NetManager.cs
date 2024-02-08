using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Player;
using Assets.Scripts.StaticData;
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
               PlayerType = playerType,
               Ammo = _playerData.Ammo,
               MoveSpeed = _playerData.MoveSpeed,
               RotateSpeed = _playerData.RotateSpeed,
               SpeedFactor = _playerData.SpeedFactor
            };
            NetworkClient.Send(message);
            _playerSpawned = true;
        }

        private async void OnCreateCharacter(NetworkConnectionToClient conn, CreatePlayerMessage message)
        {
            var player = Instantiate(_playerPrefabs[message.PlayerType], _spawnPosition, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player);
            player.GetComponent<PlayerMovement>().RpcSetSpeed(message.MoveSpeed);
            player.GetComponent<PlayerRotation>().RpcSetSpeed(message.RotateSpeed);
            player.GetComponent<PlayerAnimator>().RpcSetSpeed(message.SpeedFactor);
            player.GetComponent<PlayerCameraConnector>().Connect(player);

            //await _gameFactory.RpcUpdatePlayerData(player, _playerData);

        }
    }
}