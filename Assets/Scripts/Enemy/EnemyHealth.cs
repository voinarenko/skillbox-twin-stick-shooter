using Assets.Scripts.Logic;
using Mirror;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : NetworkBehaviour, IHealth
    {
        public EnemyAnimator Animator;

        public event Action HealthChanged;

        [field: SyncVar]
        public float Current { get; set; }

        [field: SyncVar]
        public float Max { get; set; }

        public void RpcTakeDamage(float damage)
        {
            Current -= damage;
            Animator.PlayHit();

            HealthChanged?.Invoke();
        }
    }
}