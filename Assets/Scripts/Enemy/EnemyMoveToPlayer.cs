using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    public class EnemyMoveToPlayer : MonoBehaviour
    {
        public NavMeshAgent Agent;
        public bool ApproachPlayer;
        public bool PlayerNearby;
        public event Action Completed;

        public Transform PlayerTransform;

        public void Construct(Transform playerTransform)
        {
            PlayerTransform = playerTransform;
            ApproachPlayer = true;
            //Agent.destination = PlayerTransform.position;
        }

        private void Update()
        {
            if (ApproachPlayer) SetDestinationForAgent();
            if (PlayerNearby && ApproachPlayer) CheckDistance();
        }

        private void CheckDistance()
        {
            //DEBUG
            if (Agent.gameObject.GetComponent<EnemyAttack>().Type == EnemyType.Ranged) 
                Debug.Log(Agent.remainingDistance);
            var dist=Agent.remainingDistance;
            if (!float.IsPositiveInfinity(dist) && Agent.pathStatus == NavMeshPathStatus.PathComplete &&
                Agent.remainingDistance <= Agent.stoppingDistance && PlayerTransform != null)
            {
                Completed?.Invoke();
                ApproachPlayer = false;
            }
        }

        private void SetDestinationForAgent()
        {
            if (PlayerTransform)
                Agent.destination = PlayerTransform.position;
        }
    }
}