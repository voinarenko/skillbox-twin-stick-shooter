using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.Player;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI.Elements.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Windows
{
    public class PauseWindow : BaseWindow
    {
        private const string PlayerTag = "Player";
        private const string ManagerTag = "WaveChanger";

        private SceneLoader _sceneLoader;
        private GameObject _player;
        private PauseListener _pauseListener;
        private PlayerMovement _playerMovement;
        private PlayerRotation _playerRotation;
        private PlayerShooter _playerShooter;
        [SerializeField] private ConfirmButton _confirmButton;
        [SerializeField] private RestartButton _restartButton;
        [SerializeField] private MenuReturnButton _returnButton;
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
            _restartButton.Clicked -= ProcessRestartClick;
            _returnButton.Clicked -= ProcessReturnClick;
            Cursor.visible = false;
            Time.timeScale = 1;
        }

        public override void Init()
        {
            AudioService.UpdateSliders(_masterSlider, _musicSlider, _effectsSlider);
            _confirmButton.Construct(SaveLoadService, AudioService, SettingsService);
        }

        public void SetLoader(SceneLoader sceneLoader) =>
            _sceneLoader = sceneLoader;

        protected override void Initialize()
        {
            _restartButton.Clicked += ProcessRestartClick;
            _returnButton.Clicked += ProcessReturnClick;
        }

        private void ProcessReturnClick()
        {
            Time.timeScale = 1;
            _returnButton.Clicked -= ProcessReturnClick;
            SettingsService.Settings = new Settings();
            AudioService.StoreVolume(SettingsService.Settings);
            SaveLoadService.SaveSettings();
            StateMachine.Enter<BootstrapState>();
            Destroy(gameObject);
        }

        private void ProcessRestartClick()
        {
            Time.timeScale = 1;
            _restartButton.Clicked -= ProcessRestartClick;
            SettingsService.Settings = new Settings();
            AudioService.StoreVolume(SettingsService.Settings);
            SaveLoadService.SaveSettings();
            _sceneLoader.Load("InitialScene", OnLoaded);
        }

        private void OnLoaded() => 
            StateMachine.Enter<LoadProgressState, PlayerStaticData>(StaticData.ForPlayer(Progress.PlayerState.CurrentType));
    }
}