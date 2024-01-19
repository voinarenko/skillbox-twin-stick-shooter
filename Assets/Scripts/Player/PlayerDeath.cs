using System;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
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

        public event Action Happened;

        private IPersistentProgressService _progressService;
        private bool _isDead;

        public void Construct(IPersistentProgressService progressService) => 
            _progressService = progressService;

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

            ShowResults();

            Instantiate(DeathFx, transform.position, Quaternion.identity);
        }

        private void ShowResults()
        {
            foreach (var pair in _progressService.Progress.WorldData.KillData.Killed) 
                Debug.Log($"Killed |{pair.Key}|: |{pair.Value}|");
            foreach (var pair in _progressService.Progress.WorldData.LootData.Collected) 
                Debug.Log($"Collected |{pair.Key}|: |{pair.Value}|");
        }

#pragma warning disable IDE0051

        private void OnDeath() => 
            Agent.isStopped = true;
#pragma warning restore IDE0051
    }
}