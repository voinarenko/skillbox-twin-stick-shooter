using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    public class EnemyMoveToPlayer : MonoBehaviour
    {
        public NavMeshAgent Agent;

        private Transform _playerTransform;

        public void Construct(Transform playerTransform) => 
            _playerTransform = playerTransform;

        private void Update() => 
            SetDestinationForAgent();

        private void SetDestinationForAgent()
        {
            if (_playerTransform)
                Agent.destination = _playerTransform.position;
        }
    }
}