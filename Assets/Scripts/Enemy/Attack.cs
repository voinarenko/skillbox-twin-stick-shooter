using System.Collections.Generic;
using Assets.Scripts.Logic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        public EnemyAnimator Animator;

        public float AttackCooldown = 3f;
        public float Cleavage = 0.5f;
        public List<Transform> HitPoints;
        public float Damage = 10f;

        private Transform _playerTransform;
        private float _attackCooldown;
        private bool _isAttacking;
        private int _layerMask;
        private readonly Collider[] _hits = new Collider[1];

        private bool _attackIsActive;
        public float EffectiveDistance;

        public void Construct(Transform playerTransform) => 
            _playerTransform = playerTransform;

        private void Awake() => 
            _layerMask = 1 << LayerMask.NameToLayer("Player");

        private void Update()
        {
            UpdateCooldown();
            if (CanAttack())
                StartAttack();
        }

#pragma warning disable IDE0051

        private void OnAttack()
        {
            foreach (var hitPoint in HitPoints)
            {
                if (Hit(out var hit, hitPoint))
                {
                    PhysicsDebug.DrawDebug(hitPoint.position, Cleavage, 0.1f);
                    hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
                }
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