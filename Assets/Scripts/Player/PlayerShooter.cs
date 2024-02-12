using Assets.Scripts.Bullet;
using Assets.Scripts.Data;
using Mirror;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerShooter : NetworkBehaviour
    {
        public float Damage;
        public float ShootDelay;
        public float ReloadDelay;
        public Action AmmoChanged;

        private const int AmmoConsumption = 1;
        private readonly Ammo _ammo = new();

        [SerializeField] private GameObject _shootEffectPrefab;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _shootPoint;

        private PlayerDynamicData _playerDynamicData;
        private PlayerAudio _playerAudio;
        private PlayerAnimator _playerAnimator;
        private PlayerControls _controls;

        private int _initialAmmo;
        private float _shootTime = float.MinValue;
        private float _reloadTime = float.MinValue;
        private float _shoot;
        private float _reload;

        [ClientRpc]
        public void RpcConstruct(PlayerDynamicData playerDynamicData, int ammo, float damage, float shootDelay, float reloadDelay)
        {
            _playerDynamicData = playerDynamicData;
            _initialAmmo = ammo;
            Damage = damage;
            ShootDelay = shootDelay;
            ReloadDelay = reloadDelay;
            AmmoChanged?.Invoke();
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
            if (!isOwned) return;
            _shoot = _controls.Player.Shoot.ReadValue<float>();
            _reload = _controls.Player.Reload.ReadValue<float>();

            if (_shoot > 0)
                Shoot();

            else if (_reload > 0) 
                Reload();
        }
       
#pragma warning disable IDE0051
        private void OnAttackStart() { }

        private void OnAttack()
        {
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
        }

        private void OnAttackEnded() { }

        private void OnHit() { }
       
        private void OnHitEnded() { }

#pragma warning restore IDE0051

        private void Reload()
        {
            if (!(Time.time < _reloadTime + ReloadDelay))
            {
                _reloadTime = Time.time;
                _playerDynamicData.SpentData.Reloads++;
                _playerAnimator.Reload(true);
                _playerAudio.Reload();
                _playerDynamicData.AmmoData.Available = _initialAmmo;
                AmmoChanged?.Invoke();
                _reload = 0;
            }
            else _playerAnimator.Reload(false);
        }

        private void Shoot()
        {
            if (Time.time < _shootTime + ShootDelay) return;
            if (_playerDynamicData.AmmoData.Available <= 0) return;

            _shootTime = Time.time;
            _playerDynamicData.SpentData.Bullets++;
            _playerAnimator.Shoot();
            _playerAudio.Shoot();
            _shoot = 0;
        }

        private void ConsumeAmmo()
        {
            _playerDynamicData.AmmoData.Available -= _ammo.Value;
            AmmoChanged?.Invoke();
        }
    }
}