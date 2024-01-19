using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Player
{
    public class PlayerDeath : MonoBehaviour
    {
        public NavMeshAgent Agent;
        public PlayerHealth Health;
        public PlayerMovement Move;
        public PlayerRotation Rotate;
        public PlayerShooter Attack;
        public PlayerAnimator Animator;
        public GameObject DeathFx;
        
        private bool _isDead;

        private void Start() => 
            Health.HealthChanged += HealthChanged;

        private void OnDestroy() => 
            Health.HealthChanged -= HealthChanged;

        private void HealthChanged()
        {
            if (!_isDead && Health.Current <= 0) Die();
        }

        private void Die()
        {
            _isDead = true;
            Move.enabled = false;
            Rotate.enabled = false;
            Attack.enabled = false;
            Animator.PlayDeath();

            Instantiate(DeathFx, transform.position, Quaternion.identity);
        }
#pragma warning disable IDE0051
        private void OnDeath() => 
            Agent.isStopped = true;
#pragma warning restore IDE0051
    }
}