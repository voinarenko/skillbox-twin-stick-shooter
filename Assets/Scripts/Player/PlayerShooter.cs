using Assets.Scripts.Bullet;
using Assets.Scripts.Data;
using Mirror;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerAnimator), typeof(PlayerHudConnector))]
    public class PlayerShooter : NetworkBehaviour
    {
        public PlayerDynamicData PlayerDynamicData;
        public float Damage;
        public float ShootDelay;
        public float ReloadDelay;

        private const int AmmoConsumption = 1;
        private readonly Ammo _ammo = new();

        [SerializeField] private GameObject _shootEffectPrefab;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private PlayerHudConnector _hudConnector;

        private PlayerAudio _playerAudio;
        private PlayerAnimator _playerAnimator;
        private PlayerControls _controls;

        private int _initialAmmo;
        private float _shootTime = float.MinValue;
        private float _reloadTime = float.MinValue;
        private float _shoot;
        private float _reload;

        [ClientRpc]
        public void RpcConstruct(int ammo, float damage, float shootDelay, float reloadDelay)
        {
            print("Shooter construct");
            PlayerDynamicData = new PlayerDynamicData();
            _initialAmmo = ammo;
            Damage = damage;
            ShootDelay = shootDelay;
            ReloadDelay = reloadDelay;
            PlayerDynamicData.AmmoData.Available = _initialAmmo;
            _hudConnector.PlayerAmmo = PlayerDynamicData.AmmoData.Available;
        }

        private void Start()
        {
            _playerAudio = GetComponent<PlayerAudio>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            _controls = new PlayerControls();
            _controls.Enable();
            _ammo.Value = AmmoConsumption;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            _shoot = _controls.Player.Shoot.ReadValue<float>();
            _reload = _controls.Player.Reload.ReadValue<float>();

            if (_shoot > 0)
                Shoot();

            else if (_reload > 0) 
                Reload();
        }

        #region Animation methods

#pragma warning disable IDE0051
        private void OnAttackStart() { }

        private void OnAttack() { }

        private void OnAttackEnded() { }

        private void OnHit() { }
       
        private void OnHitEnded() { }
#pragma warning restore IDE0051

        #endregion

        private void Reload()
        {
            if (!(Time.time < _reloadTime + ReloadDelay))
            {
                _reloadTime = Time.time;
                PlayerDynamicData.SpentData.Reloads++;
                _playerAnimator.Reload(true);
                _playerAudio.Reload();
                PlayerDynamicData.AmmoData.Available = _initialAmmo;
                _hudConnector.PlayerAmmo = PlayerDynamicData.AmmoData.Available;
                _reload = 0;
            }
            else _playerAnimator.Reload(false);
        }

        private void Shoot()
        {
            if (Time.time < _shootTime + ShootDelay) return;
            if (PlayerDynamicData.AmmoData.Available <= 0) return;

            _shootTime = Time.time;
            _playerAnimator.Shoot();
            _playerAudio.Shoot();
            if (_shootEffectPrefab != null)
                Instantiate(_shootEffectPrefab, _shootPoint.position, _shootPoint.rotation);
            if (_bulletPrefab != null)
            {
                var bullet = Instantiate(_bulletPrefab, _shootPoint.transform.position, transform.rotation);
                var bulletData = bullet.GetComponent<BulletDamage>();
                bulletData.Sender = tag;
                bulletData.Damage = Damage;
            }
            ConsumeAmmo();
            _shoot = 0;
        }

        private void ConsumeAmmo()
        {
            PlayerDynamicData.AmmoData.Available -= _ammo.Value;
            PlayerDynamicData.SpentData.Bullets++;
            _hudConnector.PlayerAmmo = PlayerDynamicData.AmmoData.Available;
        }
    }
}