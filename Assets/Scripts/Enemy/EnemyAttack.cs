using Assets.Scripts.Bullet;
using Assets.Scripts.Logic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Action = System.Action;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyAttack : MonoBehaviour
    {
        public NavMeshAgent Agent;
        public EnemyAudio Audio;
        public GameObject ShootEffectPrefab;
        public GameObject BulletPrefab;
        public Transform ShootPoint;
        public EnemyAnimator Animator;

        public EnemyType Type;
        public float AttackCooldown = 3f;
        public float Cleavage = 0.5f;
        public List<Transform> HitPoints;
        public float Damage = 10f;
        public event Action Completed;

        private const string PlayerLayerMask = "Player";
        private const string PlayerTag = "Player";
        private const float AttackTime = 0.1f;
        private Transform _playerTransform;
        private float _attackCooldown;
        private int _layerMask;
        private readonly Collider[] _hits = new Collider[1];

        [SerializeField] private bool _isAttacking;
        [SerializeField] private bool _attackIsActive;
        [SerializeField] private float _savedSpeed;

        public void Construct(Transform playerTransform) => 
            _playerTransform = playerTransform;

        private void Awake() => 
            _layerMask = 1 << LayerMask.NameToLayer(PlayerLayerMask);

        private void Start() => 
            _savedSpeed = Agent.speed;

        private void Update()
        {
            UpdateCooldown();
            if (CanAttack())
                StartAttack();
        }

#pragma warning disable IDE0051
        private void OnAttackStart() => 
            Agent.speed = 0;

        private void OnAttack()
        {
            if (Type == EnemyType.Ranged)
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

                Audio.Shoot();
            }
            else
            {
                foreach (var hitPoint in HitPoints)
                {
                    if (!Hit(out var hit, hitPoint)) continue;
                    PhysicsDebug.DrawDebug(hitPoint.position, Cleavage, AttackTime);
                    if (!hit.CompareTag(PlayerTag)) return;
                    hit.transform.parent.GetComponent<IHealth>().TakeDamage(Damage);

                    Audio.Attack();
                }
            }
        }

        private void OnAttackEnded()
        {
            Agent.speed = _savedSpeed;
            _attackCooldown = AttackCooldown;
            _isAttacking = false;
            if (Type == EnemyType.Ranged) Audio.Reload();
            Completed?.Invoke();
        }

        private void OnHit() => 
            Agent.speed = 0;

        private void OnHitEnded()
        {
            Agent.speed = _savedSpeed;
            Completed?.Invoke();
       }

#pragma warning restore IDE0051


        public void EnableAttack() => 
            _attackIsActive = true;

        public void DisableAttack() => 
            _attackIsActive = false;

        private void StartAttack()
        {
            transform.LookAt(_playerTransform);
            if (Type == EnemyType.Ranged)
                Animator.PlayShoot();
            else
                Animator.PlayAttack();
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