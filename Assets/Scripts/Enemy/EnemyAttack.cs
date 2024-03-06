using Assets.Scripts.Bullet;
using Assets.Scripts.Logic;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using Action = System.Action;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyAttack : NetworkBehaviour
    {
        public event Action Completed;
        public EnemyType Type { get; set; }
        public float AttackCooldown { get; set; }
        public float Cleavage { get; set; }
        public float Damage { get; set; }

        private const string PlayerLayerMask = "Player";
        private const string PlayerTag = "Player";
        private const float AttackTime = 0.1f;

        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private EnemyAudio _audio;
        [SerializeField] private GameObject _shootEffectPrefab;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private List<Transform> _hitPoints;


        private Transform _playerTransform;
        private float _attackCooldown;
        private int _layerMask;
        private readonly Collider[] _hits = new Collider[1];

        private bool _isAttacking;
        private bool _attackIsActive;
        private float _savedSpeed;

        public void Construct(Transform playerTransform) => 
            _playerTransform = playerTransform;

        private void Awake() => 
            _layerMask = 1 << LayerMask.NameToLayer(PlayerLayerMask);

        private void Start() => 
            _savedSpeed = _agent.speed;

        private void Update()
        {
            UpdateCooldown();
            if (CanAttack())
                RpcStartAttack();
        }

#pragma warning disable IDE0051
        private void OnAttackStart() => 
            _agent.speed = 0;

        [Command(requiresAuthority = false)]
        private void OnAttack()
        {
            if (Type == EnemyType.Ranged)
            {
                if (_shootEffectPrefab != null) 
                    Instantiate(_shootEffectPrefab, _shootPoint.position, _shootPoint.rotation);
                if (_bulletPrefab != null)
                {
                    var bullet = Instantiate(_bulletPrefab, _shootPoint.transform.position, transform.rotation);
                    var bulletData = bullet.GetComponent<BulletDamage>();
                    bulletData.Sender = tag;
                    bulletData.Damage = Damage;
                    NetworkServer.Spawn(bullet);
                }

                _audio.Shoot();
            }
            else
            {
                foreach (var hitPoint in _hitPoints)
                {
                    if (!Hit(out var hit, hitPoint)) continue;
                    PhysicsDebug.DrawDebug(hitPoint.position, Cleavage, AttackTime);
                    if (!hit.CompareTag(PlayerTag)) return;
                    hit.transform.parent.GetComponent<IHealth>().RpcTakeDamage(Damage);

                    _audio.Attack();
                }
            }
        }

        [Command(requiresAuthority = false)]

        private void OnAttackEnded()
        {
            _agent.speed = _savedSpeed;
            _attackCooldown = AttackCooldown;
            _isAttacking = false;
            if (Type == EnemyType.Ranged) _audio.Reload();
            Completed?.Invoke();
        }

        private void OnHit() => 
            _agent.speed = 0;

        private void OnHitEnded()
        {
            _agent.speed = _savedSpeed;
            Completed?.Invoke();
       }

#pragma warning restore IDE0051


        public void EnableAttack() => 
            _attackIsActive = true;

        public void DisableAttack() => 
            _attackIsActive = false;

        [ClientRpc]
        private void RpcStartAttack()
        {
            transform.LookAt(_playerTransform);
            if (Type == EnemyType.Ranged)
                _animator.PlayShoot();
            else
                _animator.PlayAttack();
            _isAttacking = true;
        }

        private bool Hit(out Collider hit, Transform point)
        {
            var hitsCount = Physics.OverlapSphereNonAlloc(point.position, Cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitsCount > 0;
        }

        private bool CooldownIsUp() => 
            _attackCooldown <= 0;

        private bool CanAttack() => 
            _attackIsActive && !_isAttacking && CooldownIsUp();

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _attackCooldown -= Time.deltaTime;
        }
    }
}