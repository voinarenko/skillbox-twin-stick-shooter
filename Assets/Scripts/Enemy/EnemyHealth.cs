using Assets.Scripts.Logic;
using Mirror;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : NetworkBehaviour, IHealth
    {
        public event Action HealthChanged;
        
        [SerializeField] private EnemyAnimator _animator;


        [field: SyncVar]
        public float Current { get; set; }

        [field: SyncVar]
        public float Max { get; set; }

        public void RpcTakeDamage(float damage)
        {
            Current -= damage;
            _animator.PlayHit();

            HealthChanged?.Invoke();
        }
    }
}