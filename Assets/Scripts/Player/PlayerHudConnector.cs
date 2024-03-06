using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI.Elements;
using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerHudConnector : NetworkBehaviour
    {
        public event Action<int> OnWaveNumberChanged;
        public event Action<int> OnPlayerAmmoChanged;
        public event Action<float, float> OnPlayerHealthChanged;

        private const string WaveChangerTag = "WaveChanger";
        [SerializeField] private Transform _perkParent;
        [SerializeField] private GameObject _perkTimer;


        [Header("Perk Data")]
        [SerializeField] private List<Sprite> _sprites;

        [SerializeField] private float _duration = 60f;
        [SerializeField] private float _multiplier = 1.2f;


        [Header("Player Data")]
        [SerializeField] private float _playerMaxHealth;

        private static WaveChanger Changer => GameObject.FindWithTag(WaveChangerTag).GetComponent<WaveChanger>();

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
            _waveData = waveData;
            _playerMaxHealth = maxHealth;
            WaveNumber = _waveData.Encountered;
            OnWaveNumberChanged?.Invoke(WaveNumber);
        }

        private void OnDestroy()
        {
            var timers = FindObjectsByType<PerkTimer>(FindObjectsSortMode.None);
            foreach (var timer in timers) 
                timer.Completed -= RemovePerk;
        }

        [ClientRpc]
        public void RpcGetPerk(int id)
        {
            if (isLocalPlayer)
            {
                var timer = Instantiate(_perkTimer, _perkParent).GetComponent<PerkTimer>();

                timer.Type = (PerkTypeId)id;
                timer.Icon = _sprites[id];
                timer.Duration = _duration;
                timer.Multiplier = _multiplier;

                ApplyPerk(timer, gameObject);
                timer.Completed += RemovePerk;
            }
        }

        #region Client

        public override void OnStartLocalPlayer()
        {
            _hudObject = Instantiate(_hudPrefab);
            _waveCounter = _hudObject.GetComponent<WaveCounter>();
            _ammoCounter = _hudObject.GetComponent<AmmoCounter>();
            _actorUi = _hudObject.GetComponent<ActorUi>();
            _perkParent = _hudObject.GetComponent<PerkDisplay>().GetParent();

            OnWaveNumberChanged += _waveCounter.UpdateCounter;
            OnPlayerAmmoChanged += _ammoCounter.UpdateCounter;
            OnPlayerHealthChanged += _actorUi.UpdateHealthBar;

            OnWaveNumberChanged?.Invoke(WaveNumber);
            OnPlayerAmmoChanged?.Invoke(PlayerAmmo);
            OnPlayerHealthChanged?.Invoke(PlayerHealth, _playerMaxHealth);
        }

        public override void OnStopLocalPlayer()
        {
            OnWaveNumberChanged -= _waveCounter.UpdateCounter;
            OnPlayerAmmoChanged -= _ammoCounter.UpdateCounter;
            OnPlayerHealthChanged -= _actorUi.UpdateHealthBar;
        }

        #endregion

        private void WaveNumberChanged(int _, int newWaveNumber) => 
            OnWaveNumberChanged?.Invoke(newWaveNumber);

        private void PlayerAmmoChanged(int newAmmoAmount) => 
            OnPlayerAmmoChanged?.Invoke(newAmmoAmount);

        private void PlayerHealthChanged(float newHealthAmount) => 
            OnPlayerHealthChanged?.Invoke(newHealthAmount, _playerMaxHealth);

        private static void ApplyPerk(PerkTimer timer, GameObject player)
        {
            timer.Player = player;
            var shooter = player.GetComponent<PlayerShooter>();
            switch (timer.Type)
            {
                case PerkTypeId.Damage:
                    shooter.Damage *= timer.Multiplier;
                    break;
                case PerkTypeId.Defense:
                    player.GetComponent<PlayerHealth>().Defense *= timer.Multiplier;
                    break;
                case PerkTypeId.MoveSpeed:
                    player.GetComponent<PlayerMovement>().Speed *= timer.Multiplier;
                    break;
                case PerkTypeId.AttackSpeed:
                    shooter.ShootDelay /= timer.Multiplier;
                    shooter.ReloadDelay /= timer.Multiplier;
                    break;
            }
        }

        private static void RemovePerk(PerkTimer timer, GameObject player)
        {
            var shooter = player.GetComponent<PlayerShooter>();
            switch (timer.Type)
            {
                case PerkTypeId.Damage:
                    shooter.Damage /= timer.Multiplier;
                    break;
                case PerkTypeId.Defense:
                    player.GetComponent<PlayerHealth>().Defense /= timer.Multiplier;
                    break;
                case PerkTypeId.MoveSpeed:
                    player.GetComponent<PlayerMovement>().Speed /= timer.Multiplier;
                    break;
                case PerkTypeId.AttackSpeed:
                    shooter.ShootDelay *= timer.Multiplier;
                    shooter.ReloadDelay *= timer.Multiplier;
                    break;
            }

            timer.Completed -= RemovePerk;
            Destroy(timer.gameObject);
        }

        [ClientRpc]
        public void RpcGameOver()
        {
            if (isLocalPlayer) 
                Changer.GameOver();
        }
    }
}