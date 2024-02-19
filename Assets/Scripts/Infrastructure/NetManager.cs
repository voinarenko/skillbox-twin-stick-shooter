using System.Threading.Tasks;
using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Wave;
using Assets.Scripts.Player;
using Assets.Scripts.StaticData;
using Mirror;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class NetManager : NetworkManager
    {
        private IGameFactory _gameFactory;
        private IWaveService _waveService;
        private LevelStaticData _levelStaticData;

        private PlayerStaticData _playerStaticData;
        private PlayersWatcher _playersWatcher;
        [SerializeField] private GameObject[] _playerPrefabs;

        private Vector3 _spawnPosition;

        private bool _playerSpawned;
        private bool _playerConnected;

        private WorldData _worldData;
        private IPersistentProgressService _progressService;

        public void Construct(IPersistentProgressService progressService, IGameFactory gameFactory, IWaveService waveService, LevelStaticData levelStaticData)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
            _waveService = waveService;
            _levelStaticData = levelStaticData;
            _playerStaticData = progressService.Progress.PlayerStaticData;
            _worldData = progressService.Progress.WorldData;
            //_playerStaticData = playerData;
            _spawnPosition = levelStaticData.InitialPlayerPosition;
            _playersWatcher = FindAnyObjectByType<PlayersWatcher>();
        }

        public override async void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreateCharacter);
            await CreateSpawners();
            _progressService.Progress.WorldData.WaveData.NextWave();
            _waveService.SpawnEnemies();
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
               Health = _playerStaticData.Health,
               Ammo = _playerStaticData.Ammo,
               MoveSpeed = _playerStaticData.MoveSpeed,
               RotateSpeed = _playerStaticData.RotateSpeed,
               SpeedFactor = _playerStaticData.SpeedFactor,
               Damage = _playerStaticData.Damage,
               AttackCooldown = _playerStaticData.AttackCooldown,
               ReloadCooldown = _playerStaticData.ReloadCooldown
            };
            NetworkClient.Send(message);
            _playerSpawned = true;
        }

        private async Task CreateSpawners()
        {
            foreach (var spawnerData in _levelStaticData.EnemySpawners)
            {
                var spawner = await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id);
                NetworkServer.Spawn(spawner);
            }
        }

        private void OnCreateCharacter(NetworkConnectionToClient conn, CreatePlayerMessage message)
        {
            var player = Instantiate(_playerPrefabs[message.PlayerType], _spawnPosition, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player);
            _gameFactory.RegisterProgressWatchers(player);
            _playersWatcher.AddPlayer(player.GetComponent<PlayerDeath>());
            player.GetComponent<PlayerHealth>().RpcSetHealth(message.Health);
            player.GetComponent<PlayerMovement>().RpcSetSpeed(message.MoveSpeed);
            player.GetComponent<PlayerRotation>().RpcSetSpeed(message.RotateSpeed);
            player.GetComponent<PlayerAnimator>().RpcSetSpeed(message.SpeedFactor);
            player.GetComponent<PlayerCameraConnector>().RpcConnect();
            player.GetComponent<PlayerHudConnector>().RpcConstruct(_worldData.WaveData, message.Health);
            player.GetComponent<PlayerShooter>().RpcConstruct(message.Ammo, message.Damage, message.AttackCooldown, message.ReloadCooldown);


            //await _gameFactory.CreateHud(player, _playerDynamicData);
            //var hudConnector = player.GetComponent<PlayerHudConnector>();
            //NetworkServer.Spawn(hudConnector.gameObject, player);
            //hudConnector.InitWorld(_worldData);
            //hudConnector.RpcInitPlayer();
            //hud.GetComponent<WaveCounter>().Construct(_worldData);
            //hud.GetComponent<AmmoCounter>().Construct(player, _playerDynamicData);
            //hud.GetComponent<ActorUi>().Construct(player.GetComponent<IHealth>());

            //Debug.Log($"Player's ammo: {_worldData.AmmoData.Available}");
            //await _gameFactory.UpdatePlayerData(player, _playerStaticData);

        }
    }
}