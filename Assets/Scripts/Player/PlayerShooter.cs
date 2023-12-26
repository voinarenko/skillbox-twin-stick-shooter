using Assets.Scripts.Bullet;
using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using Zenject.SpaceFighter;

namespace Assets.Scripts.Player
{
    public class PlayerShooter : MonoBehaviour, ISavedProgressReader
    {
        public GameObject ShootEffectPrefab;
        public GameObject BulletPrefab;
        public Transform ShootPoint;

        private PlayerAudio PlayerAudio => GetComponent<PlayerAudio>();
        private PlayerAnimation PlayerAnimation => GetComponent<PlayerAnimation>();
        private PlayerControls _controls;

        private float _shootTime = float.MinValue;
        [SerializeField] private float _shootDelay;

        private float _reloadTime = float.MinValue;
        [SerializeField] private float _reloadDelay;
        private float _shoot;
        private float _reload;
        private Stats _stats;

        private void Start()
        {
            _controls = new PlayerControls();
            _controls.Enable();
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
                bullet.GetComponent<BulletDamage>().Damage = _stats.Damage;
            }
        }
#pragma warning restore IDE0051

        public void LoadProgress(PlayerProgress progress) => 
            _stats = progress.PlayerStats;

        private void Reload()
        {
            if (!(Time.time < _reloadTime + _reloadDelay))
            {
                _reloadTime = Time.time;
                PlayerAnimation.Reload(true);
                PlayerAudio.Reload();
                _reload = 0;
            }
            else PlayerAnimation.Reload(false);
        }

        private void Shoot()
        {
            if (Time.time < _shootTime + _shootDelay) return;

            _shootTime = Time.time;
            PlayerAnimation.Shoot();
            PlayerAudio.Shoot();
            _shoot = 0;
        }
    }
}