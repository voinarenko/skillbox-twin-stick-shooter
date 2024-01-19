using Assets.Scripts.Enemy.UtilityAi;
using System.Collections;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
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
        private EnemyHealth Health => GetComponent<EnemyHealth>();
        private EnemyAnimator Animator => GetComponent<EnemyAnimator>();
        private NavMeshAgent Agent => GetComponent<NavMeshAgent>();
        private AiBrain AiBrain => GetComponent<AiBrain>();
        private EnemyAttack Attack => GetComponent<EnemyAttack>();

        private EnemyBehavior Behavior => GetComponent<EnemyBehavior>();

        public void Construct(IPersistentProgressService progressService) => 
            _progressService = progressService;

        private void Start() => 
            Health.HealthChanged += HealthChanged;

        private void HealthChanged()
        {
            if (Health.Current <= 0)
                Die();
        }

        private void Die()
        {
            Agent.isStopped = true;
            _progressService.Progress.WorldData.WaveData.RemoveEnemy();
            _progressService.Progress.WorldData.KillData.Collect(Attack);
            _progressService.Progress.WorldData.ScoreData.UpdateScore(this);

            Attack.enabled = false;
            GetComponentInChildren<Collider>().enabled = false;
            Health.HealthChanged -= HealthChanged;
            AiBrain.SetAction(Behavior.ActionsAvailable[2]);
            Animator.PlayDeath();
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
            Agent.isStopped = true;
#pragma warning restore IDE0051
    }
}