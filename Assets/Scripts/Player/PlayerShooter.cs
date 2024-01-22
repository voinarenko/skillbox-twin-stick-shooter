using Assets.Scripts.Bullet;
using Assets.Scripts.Data;
using Assets.Scripts.StaticData;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerShooter : MonoBehaviour
    {
        public GameObject ShootEffectPrefab;
        public GameObject BulletPrefab;
        public Transform ShootPoint;

        private const int AmmoConsumption = 1;
        private readonly Ammo _ammo = new();
        private PlayerStaticData _playerStaticData;
        private PlayerAudio PlayerAudio => GetComponent<PlayerAudio>();
        private PlayerAnimator PlayerAnimator => GetComponent<PlayerAnimator>();
        private PlayerControls _controls;
        private WorldData _worldData;

        public float Damage;

        private float _shootTime = float.MinValue;
        public float ShootDelay;

        private float _reloadTime = float.MinValue;
        public float ReloadDelay;
        private float _shoot;
        private float _reload;

        public void Construct(PlayerStaticData playerStaticData, WorldData worldData, float damage, float shootDelay, float reloadDelay)
        {
            _playerStaticData = playerStaticData;
            _worldData = worldData;
            Damage = damage;
            ShootDelay = shootDelay;
            ReloadDelay = reloadDelay;
        }

        private void Start()
        {
            _controls = new PlayerControls();
            _controls.Enable();
            _ammo.Value = AmmoConsumption;
        }

        private void Update()
        {
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
            if (ShootEffectPrefab != null) 
                Instantiate(ShootEffectPrefab, ShootPoint.position, ShootPoint.rotation);
            if (BulletPrefab != null)
            {
                var bullet = Instantiate(BulletPrefab, ShootPoint.transform.position, transform.rotation);
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
                _worldData.SpentData.Reloads++;
                PlayerAnimator.Reload(true);
                PlayerAudio.Reload();
                _worldData.AmmoData.Available = _playerStaticData.Ammo;
                _worldData.AmmoData.Changed?.Invoke();
                _reload = 0;
            }
            else PlayerAnimator.Reload(false);
        }

        private void Shoot()
        {
            if (Time.time < _shootTime + ShootDelay) return;
            if (_worldData.AmmoData.Available <= 0) return;

            _shootTime = Time.time;
            _worldData.SpentData.Bullets++;
            PlayerAnimator.Shoot();
            PlayerAudio.Shoot();
            _shoot = 0;
        }

        private void ConsumeAmmo() => 
            _worldData.AmmoData.Consume(_ammo);
    }
}