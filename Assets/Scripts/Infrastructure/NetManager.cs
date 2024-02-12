using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Player;
using Assets.Scripts.StaticData;
using Mirror;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class NetManager : NetworkManager
    {
        private IGameFactory _gameFactory;

        private PlayerStaticData _playerStaticData;
        private PlayerDynamicData _playerDynamicData;
        [SerializeField] private GameObject[] _playerPrefabs;

        private Vector3 _spawnPosition;
        
        private bool _playerSpawned;
        private bool _playerConnected;
        //private IPersistentProgressService _progressService;

        public void Construct(IPersistentProgressService progressService, IGameFactory gameFactory, /*PlayerStaticData playerData, */Vector3 position)
        {
            //_progressService = progressService;
            _gameFactory = gameFactory;
            _playerStaticData = progressService.Progress.PlayerStaticData;
            _playerDynamicData = progressService.Progress.PlayerDynamicData;
            //_playerStaticData = playerData;
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
            var playerType = (int)_playerStaticData.PlayerTypeId;
            var message = new CreatePlayerMessage
            {
               PlayerType = playerType,
               Ammo = _playerStaticData.Ammo,
               MoveSpeed = _playerStaticData.MoveSpeed,
               RotateSpeed = _playerStaticData.RotateSpeed,
               SpeedFactor = _playerStaticData.SpeedFactor
            };
            NetworkClient.Send(message);
            _playerSpawned = true;
        }

        private void OnCreateCharacter(NetworkConnectionToClient conn, CreatePlayerMessage message)
        {
            var player = Instantiate(_playerPrefabs[message.PlayerType], _spawnPosition, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player);
            _gameFactory.RegisterProgressWatchers(player);
            player.GetComponent<PlayerMovement>().RpcSetSpeed(message.MoveSpeed);
            player.GetComponent<PlayerRotation>().RpcSetSpeed(message.RotateSpeed);
            player.GetComponent<PlayerAnimator>().RpcSetSpeed(message.SpeedFactor);
            player.GetComponent<PlayerCameraConnector>().RpcConnect(player);
            player.GetComponent<PlayerShooter>().RpcConstruct(_playerDynamicData, _playerStaticData.Ammo, _playerStaticData.Damage, _playerStaticData.AttackCooldown, _playerStaticData.ReloadCooldown);
            //Debug.Log($"Player's ammo: {_worldData.AmmoData.Available}");
            //await _gameFactory.UpdatePlayerData(player, _playerStaticData);

        }
    }
}