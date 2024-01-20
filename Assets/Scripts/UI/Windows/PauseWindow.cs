using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.Logic.EnemySpawners;
using Assets.Scripts.Player;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI.Elements.Buttons;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Windows
{
    public class PauseWindow : BaseWindow
    {
        private const string PlayerTag = "Player";
        private const string ManagerTag = "WaveChanger";
        public ConfirmButton ConfirmButton;
        public RestartButton RestartButton;
        public MenuReturnButton ReturnButton;

        private SceneLoader _sceneLoader;
        private GameObject _player;
        private PauseListener _pauseListener;
        private PlayerMovement _playerMovement;
        private PlayerRotation _playerRotation;
        private PlayerShooter _playerShooter;
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _effectsSlider;

        protected override void Start()
        {
            base.Start();
            _player = GameObject.FindWithTag(PlayerTag);
            _pauseListener = GameObject.FindWithTag(ManagerTag).GetComponent<PauseListener>();
            _playerMovement = _player.GetComponent<PlayerMovement>();
            _playerRotation = _player.GetComponent<PlayerRotation>();
            _playerShooter = _player.GetComponent<PlayerShooter>();
            _playerMovement.enabled = false;
            _playerRotation.enabled = false;
            _playerShooter.enabled = false;
            Time.timeScale = 0;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _pauseListener.CancelPause();
            if (_playerMovement) _playerMovement.enabled = true;
            if (_playerRotation) _playerRotation.enabled = true;
            if (_playerShooter) _playerShooter.enabled = true;
            Cursor.visible = false;
            Time.timeScale = 1;
        }

        public override void Init()
        {
            AudioService.UpdateSliders(_masterSlider, _musicSlider, _effectsSlider);
            ConfirmButton.Construct(SaveLoadService, AudioService, SettingsService);
        }

        public void SetLoader(SceneLoader sceneLoader) =>
            _sceneLoader = sceneLoader;

        protected override void Initialize()
        {
            RestartButton.Clicked += ProcessRestartClick;
            ReturnButton.Clicked += ProcessReturnClick;
        }

        private void ProcessReturnClick()
        {
            Time.timeScale = 1;
            ReturnButton.Clicked -= ProcessReturnClick;
            SettingsService.Settings = new Settings();
            AudioService.StoreVolume(SettingsService.Settings);
            SaveLoadService.SaveSettings();
            StateMachine.Enter<BootstrapState>();
            Destroy(gameObject);
        }

        private void ProcessRestartClick()
        {
            Time.timeScale = 1;
            ReturnButton.Clicked -= ProcessRestartClick;
            SettingsService.Settings = new Settings();
            AudioService.StoreVolume(SettingsService.Settings);
            SaveLoadService.SaveSettings();
            _sceneLoader.Load("InitialScene", OnLoaded);
            //Destroy(_player);
            //foreach (var point in FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None)) Destroy(point);
            //Destroy(gameObject);
        }

        private void OnLoaded() => 
            StateMachine.Enter<LoadProgressState, PlayerStaticData>(StaticData.ForPlayer(Progress.PlayerState.CurrentType));
    }
}