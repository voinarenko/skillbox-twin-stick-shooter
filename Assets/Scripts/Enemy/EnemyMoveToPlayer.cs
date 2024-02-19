using Assets.Scripts.Player;
using Mirror;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    public class EnemyMoveToPlayer : NetworkBehaviour
    {
        public NavMeshAgent Agent;
        public Transform PlayerTransform;

        public bool PlayerNearby;
        public event Action Completed;

        private const string PlayerTag = "Player";
        private EnemyBehavior _behavior;
        private EnemyAttack _attack;

        private void Start()
        {
            _behavior = GetComponent<EnemyBehavior>();
            _attack = GetComponent<EnemyAttack>();
        }

        private void Update()
        {
            if (!isServer) return;
            if (PlayerTransform == null)
            {
                var player = GameObject.FindWithTag(PlayerTag);
                if (player != null) {
                    PlayerTransform = player.GetComponent<PlayerMovement>().transform;
                    player.GetComponent<PlayerDeath>().Happened += PlayerKilled;
                    _behavior.PlayerHealth = player.GetComponent<PlayerHealth>();
                    _attack.Construct(PlayerTransform);
                }
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