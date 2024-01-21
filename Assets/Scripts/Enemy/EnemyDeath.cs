using Assets.Scripts.Enemy.UtilityAi;
using System.Collections;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Action = System.Action;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator), typeof(NavMeshAgent))]
    public class EnemyDeath : MonoBehaviour
    {
        public GameObject DeathFx;
        public event Action Happened;
        public int Value;

        private const float TimeToDestroy = 3;
        private const float TimeToSpawnLoot = 2.5f;
        private IPersistentProgressService _progressService;
        private EnemyMoveToPlayer _mover;
        private EnemyHealth _health;
        private EnemyAnimator _animator;
        private NavMeshAgent _agent;
        private AiBrain _aiBrain;
        private EnemyAttack _attack;
        private EnemyBehavior _behavior;
        private BoxCollider _collider;

        public void Construct(IPersistentProgressService progressService) => 
            _progressService = progressService;

        private void Start()
        {
            _mover = GetComponent<EnemyMoveToPlayer>();
            _health = GetComponent<EnemyHealth>();
            _animator = GetComponent<EnemyAnimator>();
            _agent = GetComponent<NavMeshAgent>();
            _aiBrain = GetComponent<AiBrain>();
            _attack = GetComponent<EnemyAttack>();
            _behavior = GetComponent<EnemyBehavior>();
            _collider = GetComponentInChildren<BoxCollider>();

            _health.HealthChanged += HealthChanged;
        }

        private void HealthChanged()
        {
            if (_health.Current <= 0)
                Die();
        }

        private void Die()
        {
            _collider.enabled = false;
            _health.HealthChanged -= HealthChanged;
            _progressService.Progress.WorldData.WaveData.RemoveEnemy();
            _progressService.Progress.WorldData.KillData.Collect(_attack);
            _progressService.Progress.WorldData.ScoreData.UpdateScore(this);
            _aiBrain.SetAction(_behavior.ActionsAvailable[2]);
            _mover.enabled = false;
            _aiBrain.enabled = false;
            _agent.updatePosition = false;
            _agent.updateRotation = false;
            _attack.enabled = false;
            _animator.PlayDeath();
            SpawnDeathFx();
            StartCoroutine(Inform());
            Destroy(gameObject, TimeToDestroy);
        }

        private IEnumerator Inform()
        {
            yield return new WaitForSeconds(TimeToSpawnLoot);
            Happened?.Invoke();
        }

        private void SpawnDeathFx() => 
            Instantiate(DeathFx, transform.position, Quaternion.identity);

#pragma warning disable IDE0051
        private void OnDeath() => 
            _agent.isStopped = true;
#pragma warning restore IDE0051
    }
}