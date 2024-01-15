using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(Attack))]
    public class EnemyMoveToPlayer : MonoBehaviour
    {
        public NavMeshAgent Agent;
        public bool PlayerNearby;

        private const float RemainingDistanceThreshold = 0.3f;
        private Attack Attack => GetComponent<Attack>();
        private Transform _playerTransform;

        public void Construct(Transform playerTransform) => 
            _playerTransform = playerTransform;

        private void Update()
        {
            SetDestinationForAgent();
            if (PlayerNearby) CheckDistance();
        }

        private void CheckDistance()
        {
            var dist=Agent.remainingDistance;
            if (!float.IsPositiveInfinity(dist) && Agent.pathStatus == NavMeshPathStatus.PathComplete &&
                Agent.remainingDistance <= RemainingDistanceThreshold)
                Attack.EnableAttack();
            else Attack.DisableAttack();
        }

        private void SetDestinationForAgent()
        {
            if (_playerTransform)
                Agent.destination = _playerTransform.position;
        }
    }
}