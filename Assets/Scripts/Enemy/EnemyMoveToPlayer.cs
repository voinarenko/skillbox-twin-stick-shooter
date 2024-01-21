using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    public class EnemyMoveToPlayer : MonoBehaviour
    {
        public NavMeshAgent Agent;
        public bool PlayerNearby;
        public event Action Completed;

        public Transform PlayerTransform;

        public void Construct(Transform playerTransform) => 
            PlayerTransform = playerTransform;

        private void Update()
        {
            SetDestinationForAgent();
            if (PlayerNearby) CheckDistance();
        }

        private void CheckDistance()
        {
            var dist = Agent.remainingDistance;
            if (!float.IsPositiveInfinity(dist) && Agent.remainingDistance <= Agent.stoppingDistance && PlayerTransform != null) 
                Completed?.Invoke();
        }

        public void SetDestinationForAgent()
        {
            if (PlayerTransform)
                Agent.destination = PlayerTransform.position;
        }
    }
}