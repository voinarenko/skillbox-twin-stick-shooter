using Mirror;
using System;
using UnityEngine;
using UnityEngine.AI;
using NetworkServer = Mirror.NetworkServer;

namespace Assets.Scripts.Player
{
    public class PlayerDeath : NetworkBehaviour
    {
        private const string DeadTag = "Dead";
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private PlayerHealth _health;
        [SerializeField] private PlayerMovement _move;
        [SerializeField] private PlayerRotation _rotate;
        [SerializeField] private PlayerShooter _attack;
        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private GameObject _deathFx;

        public event Action<PlayerDeath> Happened;

        private bool _isDead;

        private void Start() =>
            _health.HealthChanged += HealthChanged;

        private void OnDestroy() => 
            _health.HealthChanged -= HealthChanged;

        private void HealthChanged()
        {
            if (!_isDead && _health.Current <= 0) Die();
        }

        private void Die()
        {
            _isDead = true;
            _move.enabled = false;
            _rotate.enabled = false;
            _attack.enabled = false;
            _animator.PlayDeath();
            tag = DeadTag;
            Happened?.Invoke(this);

            SpawnEffect();
        }

        [Server]
        private void SpawnEffect()
        {
            var effect = Instantiate(_deathFx, transform.position, Quaternion.identity);
            NetworkServer.Spawn(effect);
        }

#pragma warning disable IDE0051

        private void OnDeath() => 
            _agent.isStopped = true;
#pragma warning restore IDE0051
    }
}