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
        private PlayerAnimation PlayerAnimation => GetComponent<PlayerAnimation>();
        private PlayerControls _controls;
        private WorldData _worldData;

        private float _damage;

        private float _shootTime = float.MinValue;
        [SerializeField] private float _shootDelay;

        private float _reloadTime = float.MinValue;
        [SerializeField] private float _reloadDelay;
        private float _shoot;
        private float _reload;

        public void Construct(PlayerStaticData playerStaticData, WorldData worldData, float damage, float shootDelay, float reloadDelay)
        {
            _playerStaticData = playerStaticData;
            _worldData = worldData;
            _damage = damage;
            _shootDelay = shootDelay;
            _reloadDelay = reloadDelay;
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
        private void OnAttack()
        {            
            if (ShootEffectPrefab != null) 
                Instantiate(ShootEffectPrefab, ShootPoint.position, ShootPoint.rotation);
            if (BulletPrefab != null)
            {
                var bullet = Instantiate(BulletPrefab, ShootPoint.transform.position, transform.rotation);
                var bulletData = bullet.GetComponent<BulletDamage>();
                bulletData.Sender = tag;
                bulletData.Damage = _damage;
            }
            ConsumeAmmo();
        }

        private void OnAttackEnded()
        {
            
        }
#pragma warning restore IDE0051

        private void Reload()
        {
            if (!(Time.time < _reloadTime + _reloadDelay))
            {
                _reloadTime = Time.time;
                PlayerAnimation.Reload(true);
                PlayerAudio.Reload();
                _worldData.AmmoData.Available = _playerStaticData.Ammo;
                _worldData.AmmoData.Changed?.Invoke();
                _reload = 0;
            }
            else PlayerAnimation.Reload(false);
        }

        private void Shoot()
        {
            if (Time.time < _shootTime + _shootDelay) return;
            if (_worldData.AmmoData.Available <= 0) return;

            _shootTime = Time.time;
            PlayerAnimation.Shoot();
            PlayerAudio.Shoot();
            _shoot = 0;
        }

        private void ConsumeAmmo() => 
            _worldData.AmmoData.Consume(_ammo);
    }
}