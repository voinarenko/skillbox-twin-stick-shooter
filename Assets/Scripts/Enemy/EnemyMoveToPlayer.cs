using Assets.Scripts.Player;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    public class EnemyMoveToPlayer : MonoBehaviour
    {
        public NavMeshAgent Agent;
        public Transform PlayerTransform;

        public bool PlayerNearby;
        public event Action Completed;

        private const string PlayerTag = "Player";


        public void Construct(Transform playerTransform)
        {
            PlayerTransform = playerTransform;
            playerTransform.GetComponent<PlayerDeath>().Happened += PlayerKilled;
        }

        private void Update()
        {
            if (PlayerTransform == null)
            {
                var player = GameObject.FindWithTag(PlayerTag);
                PlayerTransform = player.GetComponent<PlayerMovement>().transform;
                player.GetComponent<PlayerDeath>().Happened += PlayerKilled;
            }
            SetDestinationForAgent();
            if (PlayerNearby) CheckDistance();
        }

        private void PlayerKilled(PlayerDeath player)
        {
            player.Happened -= PlayerKilled;
            PlayerTransform = null;
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