using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI.Elements.Buttons;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class PlayerSelector : MonoBehaviour
    {
        private readonly Array _playerTypes = Enum.GetValues(typeof(PlayerTypeId));
        
        [SerializeField] private LeftButton _leftButton;
        [SerializeField] private RightButton _rightButton;
        [SerializeField] private PlayButton PlayButton;

        [SerializeField] private TextMeshProUGUI _playerTypeText;
        [SerializeField] private Image _playerImage;

        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Slider _damageSlider;
        [SerializeField] private Slider _ammoSlider;
        [SerializeField] private Slider _speedSlider;
        [SerializeField] private Slider _shootSlider;
        [SerializeField] private Slider _reloadSlider;

        private IStaticDataService _staticData;
        private IGameStateMachine _stateMachine;
        private int _playerType;

        public void Construct(IStaticDataService staticData, IGameStateMachine stateMachine)
        {
            _staticData = staticData;
            _stateMachine = stateMachine;
            UpdateData(0);
        }

        private void Awake()
        {
            _leftButton.Clicked += SwitchLeft;
            _rightButton.Clicked += SwitchRight;
            PlayButton.Clicked += Play;
        }

        private void OnDestroy()
        {
            _leftButton.Clicked -= SwitchLeft;
            _rightButton.Clicked -= SwitchRight;
            PlayButton.Clicked -= Play;
        }

        private void SwitchLeft()
        {
            _playerType--;
            if (_playerType == -1) _playerType = _playerTypes.Length - 1;
            UpdateData(_playerType);
        }

        private void SwitchRight()
        {
            _playerType++;
            if (_playerType == _playerTypes.Length) _playerType = 0;
            UpdateData(_playerType);
        }

        private void Play() => 
            _stateMachine.Enter<LoadProgressState, PlayerStaticData>(_staticData.ForPlayer((PlayerTypeId)_playerType));

        private void UpdateData(int playerTypeId)
        {
            var playerStaticData = _staticData.ForPlayer((PlayerTypeId)playerTypeId);
            _playerTypeText.text = playerStaticData.PlayerTypeId.ToString();
            _playerImage.sprite = playerStaticData.Image;
            _healthSlider.value = playerStaticData.Health;
            _damageSlider.value = playerStaticData.Damage;
            _ammoSlider.value = playerStaticData.Ammo;
            _speedSlider.value = playerStaticData.MoveSpeed;
            _shootSlider.value = playerStaticData.AttackCooldown;
            _reloadSlider.value = playerStaticData.ReloadCooldown;
        }
    }
}