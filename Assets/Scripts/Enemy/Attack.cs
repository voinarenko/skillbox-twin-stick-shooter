using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services;
using System.Linq;
using Assets.Scripts.Logic;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        public EnemyAnimator Animator;

        public float AttackCooldown = 3f;
        public float Cleavage = 0.5f;
        public Transform HitPoint;
        public float Damage = 10f;

        private IGameFactory _factory;
        private Transform _playerTransform;
        private float _attackCooldown;
        private bool _isAttacking;
        private int _layerMask;
        private readonly Collider[] _hits = new Collider[1];
        private bool _attackIsActive;

        private void Awake()
        {
            _factory = AllServices.Container.Single<IGameFactory>();
            _factory.PlayerCreated += OnPlayerCreated;

            _layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            UpdateCooldown();
            if (CanAttack())
                StartAttack();
        }

#pragma warning disable IDE0051
        private void OnAttack()
        {
            if (Hit(out var hit))
            {
                PhysicsDebug.DrawDebug(HitPoint.position, Cleavage, 1);
                hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
            }
        }

        private void OnAttackEnded()
        {
            _attackCooldown = AttackCooldown;
            _isAttacking = false;
        }
#pragma warning restore IDE0051

        public void EnableAttack() => 
            _attackIsActive = true;

        public void DisableAttack() => 
            _attackIsActive = false;

        private void StartAttack()
        {
            transform.LookAt(_playerTransform);
            Animator.PlayAttack();

            _isAttacking = true;
        }

        private bool Hit(out Collider hit)
        {
            var hitsCount = Physics.OverlapSphereNonAlloc(HitPoint.transform.position, Cleavage, _hits, _layerMask);
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

        private void OnPlayerCreated() => 
            _playerTransform = _factory.PlayerGameObject.transform;
    }
}