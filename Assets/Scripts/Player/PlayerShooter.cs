﻿using Assets.Scripts.Bullet;
using Assets.Scripts.Data;
using Mirror;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerAnimator), typeof(PlayerHudConnector))]
    public class PlayerShooter : NetworkBehaviour
    {
        public DataStorage Storage { get; private set; }

        public float Damage { get; set; }
        public float ShootDelay { get; set; }
        public float ReloadDelay { get; set; }

        private const int AmmoConsumption = 1;
        private const string StorageTag = "Storage";
        private readonly Ammo _ammo = new();

        [SerializeField] private GameObject _shootEffectPrefab;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private PlayerHudConnector _hudConnector;

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
        public void RpcConstruct(int ammo, float damage, float shootDelay, float reloadDelay)
        {
            _initialAmmo = ammo;
            Damage = damage;
            ShootDelay = shootDelay;
            ReloadDelay = reloadDelay;
        }

        private void Start()
        {
            Storage = GameObject.FindWithTag(StorageTag).GetComponent<DataStorage>();
            _playerDynamicData = Storage.PlayerDynamicData;
            _playerDynamicData.AmmoData.Available = _initialAmmo;
            _playerAudio = GetComponent<PlayerAudio>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            _controls = new PlayerControls();
            _controls.Enable();
            _ammo.Value = AmmoConsumption;
            _hudConnector.PlayerAmmo = _playerDynamicData.AmmoData.Available;
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
                ReplenishAmmo();
                OnReload();
                _hudConnector.PlayerAmmo = _playerDynamicData.AmmoData.Available;
                _reload = 0;
            }
            else _playerAnimator.Reload(false);
        }

        private void Shoot()
        {
            if (Time.time < _shootTime + ShootDelay) return;
            if (_playerDynamicData.AmmoData.Available <= 0) return;

            _shootTime = Time.time;
            CmdFire();
            ConsumeAmmo();
            _shoot = 0;
        }

        private void ReplenishAmmo()
        {
            if(!isLocalPlayer) return;
            _playerDynamicData.SpentData.Reloads++;
            Storage.PlayerDynamicData.AmmoData.Available = _initialAmmo;
        }

        private void ConsumeAmmo()
        {
            if(!isLocalPlayer) return;
            Storage.PlayerDynamicData.AmmoData.Available -= _ammo.Value;
            Storage.PlayerDynamicData.SpentData.Bullets++;
            _hudConnector.PlayerAmmo = _playerDynamicData.AmmoData.Available;
        }

        [Command]
        private void CmdFire()
        {
            if (_shootEffectPrefab != null)
            {
                var effect = Instantiate(_shootEffectPrefab, _shootPoint.position, _shootPoint.rotation);
                NetworkServer.Spawn(effect);
            }

            if (_bulletPrefab != null)
            {
                var bullet = Instantiate(_bulletPrefab, _shootPoint.transform.position, transform.rotation);
                var bulletData = bullet.GetComponent<BulletDamage>();
                bulletData.Sender = tag;
                bulletData.Damage = Damage;
                NetworkServer.Spawn(bullet);
            }

            RpcOnFire();
        }

        [ClientRpc]
        private void RpcOnFire()
        {
            _playerAnimator.Shoot();
            _playerAudio.Shoot();
        }

        private void OnReload()
        {
            _playerAnimator.Reload(true);
            _playerAudio.Reload();
        }
    }
}