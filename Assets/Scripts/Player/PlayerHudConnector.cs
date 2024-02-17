using Assets.Scripts.UI.Elements;
using Mirror;
using System;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerHudConnector : NetworkBehaviour
    {
        public event Action<int> OnWaveNumberChanged;
        public event Action<int> OnPlayerAmmoChanged;
        public event Action<float, float> OnPlayerHealthChanged;

        [SerializeField] private float _playerMaxHealth;

        private int _playerAmmo;
        public int PlayerAmmo
        {
            get => _playerAmmo;
            set
            {
                if (_playerAmmo == value) return;
                _playerAmmo = value;
                PlayerAmmoChanged(value);
            }
        }

        private float _playerHealth;
        public float PlayerHealth
        {
            get => _playerHealth;
            set
            {
                if (Math.Abs(_playerHealth - value) < 0.001f) return;
                _playerHealth = value;
                PlayerHealthChanged(value);
            }
        }

        #region SyncVars

        [Header("SyncVars")] 
        [SyncVar(hook = nameof(WaveNumberChanged))]
        public int WaveNumber;

        #endregion

        [Header("Links")]
        [SerializeField] private GameObject _hudPrefab;

        private GameObject _hudObject;
        private WaveCounter _waveCounter;
        private AmmoCounter _ammoCounter;
        private ActorUi _actorUi;
        private WaveData _waveData;

        [ClientRpc]
        public void RpcConstruct(WaveData waveData, int maxHealth)
        {
            print($"HUD connector construct");
            _waveData = waveData;
            _playerMaxHealth = maxHealth;
            _waveData.Changed += OnWaveNumberChanged;
        }

        #region Client

        public override void OnStartLocalPlayer()
        {
            _hudObject = Instantiate(_hudPrefab);
            _waveCounter = _hudObject.GetComponent<WaveCounter>();
            _ammoCounter = _hudObject.GetComponent<AmmoCounter>();
            _actorUi = _hudObject.GetComponent<ActorUi>();

            OnWaveNumberChanged += _waveCounter.UpdateCounter;
            OnPlayerAmmoChanged += _ammoCounter.UpdateCounter;
            OnPlayerHealthChanged += _actorUi.UpdateHealthBar;

            OnWaveNumberChanged?.Invoke(WaveNumber);
            OnPlayerAmmoChanged?.Invoke(PlayerAmmo);
            OnPlayerHealthChanged?.Invoke(PlayerHealth, _playerMaxHealth);
        }

        public override void OnStopLocalPlayer()
        {
            _waveData.Changed -= OnWaveNumberChanged;
            OnWaveNumberChanged -= _waveCounter.UpdateCounter;
            OnPlayerAmmoChanged -= _ammoCounter.UpdateCounter;
            OnPlayerHealthChanged -= _actorUi.UpdateHealthBar;
        }

        #endregion

        private void WaveNumberChanged(int _, int newWaveNumber) => 
            OnWaveNumberChanged?.Invoke(newWaveNumber);

        private void PlayerAmmoChanged(int newAmmoAmount)
        {
            print($"Ammo changed to |{newAmmoAmount}|");
            OnPlayerAmmoChanged?.Invoke(newAmmoAmount);
        }

        private void PlayerHealthChanged(float newHealthAmount) => 
            OnPlayerHealthChanged?.Invoke(newHealthAmount, _playerMaxHealth);
    }
}