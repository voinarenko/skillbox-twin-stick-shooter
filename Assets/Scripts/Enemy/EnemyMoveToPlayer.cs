using Assets.Scripts.Player;
using Mirror;
using System;
using System.Linq;
using Assets.Scripts.Infrastructure;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    public class EnemyMoveToPlayer : NetworkBehaviour
    {
        public bool PlayerNearby { get; set; }
        public NavMeshAgent Agent;
        public event Action Completed;

        private PlayersWatcher _playersWatcher;
        private Transform _playerTransform;
        private EnemyBehavior _behavior;
        private EnemyAttack _attack;

        private void Start()
        {
            _playersWatcher = FindAnyObjectByType<PlayersWatcher>();
            _behavior = GetComponent<EnemyBehavior>();
            _attack = GetComponent<EnemyAttack>();
        }

        private void Update()
        {
            if (!isServer) return;
            if (_playerTransform == null)
            {
                var player = FindTarget();
                if (player != null) 
                    InitTarget(player);
            }
            else
            {
                SetDestinationForAgent();
                if (PlayerNearby) CheckDistance();
            }
        }

        public void InitTarget(GameObject player)
        {
            _playerTransform = player.GetComponent<PlayerMovement>().transform;
            player.GetComponent<PlayerDeath>().Happened += PlayerKilled;
            _behavior.PlayerHealth = player.GetComponent<PlayerHealth>();
            _attack.Construct(_playerTransform);
        }

        public void SetDestinationForAgent()
        {
            if (_playerTransform)
                Agent.destination = _playerTransform.position;
        }

        private GameObject FindTarget()
        {
            GameObject target = null;
            var targets = _playersWatcher.GetConnectors();
            if (targets.Count > 0)
            {
                target = targets.FirstOrDefault()!.gameObject;
                if (targets.Count > 1 && target != null)
                {
                    Agent.destination = target.transform.position;
                    var distance = Agent.remainingDistance;
                    foreach (var t in targets)
                    {
                        Agent.destination = t.transform.position;
                        var newDistance = Agent.remainingDistance;
                        if (newDistance < distance) target = t.gameObject;
                    }
                }
            }
            return target;
        }

        private void PlayerKilled(PlayerDeath player)
        {
            player.Happened -= PlayerKilled;
            _playerTransform = null;
        }

        private void CheckDistance()
        {
            var dist = Agent.remainingDistance;
            if (!float.IsPositiveInfinity(dist) && Agent.remainingDistance <= Agent.stoppingDistance && _playerTransform != null) 
                Completed?.Invoke();
        }
    }
}