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

        private const float TimeToDestroy = 3;
        private const float TimeToSpawnLoot = 2.5f;
        private EnemyHealth Health => GetComponent<EnemyHealth>();
        private EnemyAnimator Animator => GetComponent<EnemyAnimator>();
        private NavMeshAgent Agent => GetComponent<NavMeshAgent>();
        private AiBrain AiBrain => GetComponent<AiBrain>();
        private EnemyBehavior Behavior => GetComponent<EnemyBehavior>();

        private void Start() => 
            Health.HealthChanged += HealthChanged;

        private void HealthChanged()
        {
            if (Health.Current <= 0)
                Die();
        }

        private void Die()
        {
            GetComponent<EnemyAttack>().enabled = false;
            GetComponentInChildren<Collider>().enabled = false;
            Health.HealthChanged -= HealthChanged;
            AiBrain.SetAction(Behavior.ActionsAvailable[2]);
            Agent.isStopped = true;
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
    }
}