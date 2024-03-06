using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyAnimator))]
    public class AnimateAlongAgent : NetworkBehaviour
    {
        private const float MinimalVelocity = 0.1f;
        
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private EnemyAnimator _animator;

        private void Update()
        {
            if (!isServer) return;
            if(ShouldMove())
                _animator.Move();
            else
                _animator.StopMoving();
        }

        private bool ShouldMove() => 
            _agent.velocity.magnitude > MinimalVelocity && _agent.remainingDistance > _agent.radius;
    }
}