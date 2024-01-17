using Assets.Scripts.Enemy.UtilityAi;
using System.Collections;
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

        private EnemyHealth Health => GetComponent<EnemyHealth>();
        private EnemyAnimator Animator => GetComponent<EnemyAnimator>();
        private NavMeshAgent Agent => GetComponent<NavMeshAgent>();
        private AiBrain AiBrain => GetComponent<AiBrain>();
        private EnemyBehavior Behavior => GetComponent<EnemyBehavior>();

        private void Start() => 
            Health.HealthChanged += HealthChanged;

        private void OnDestroy() => 
            Health.HealthChanged -= HealthChanged;

        private void HealthChanged()
        {
            if (Health.Current <= 0)
                Die();
        }

        private void Die()
        {
            Health.HealthChanged -= HealthChanged;
            AiBrain.SetAction(Behavior.ActionsAvailable[2]);
            Agent.isStopped = true;
            Animator.PlayDeath();
            SpawnDeathFx();
            StartCoroutine(DestroyTimer());

            Happened?.Invoke();
        }

        private void SpawnDeathFx() => 
            Instantiate(DeathFx, transform.position, Quaternion.identity);

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}