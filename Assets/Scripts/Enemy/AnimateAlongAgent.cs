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
        
        public NavMeshAgent Agent;
        public EnemyAnimator Animator;

        private void Update()
        {
            if (!isServer) return;
            if(ShouldMove())
                Animator.Move();
            else
                Animator.StopMoving();
        }

        private bool ShouldMove() => 
            Agent.velocity.magnitude > MinimalVelocity && Agent.remainingDistance > Agent.radius;
    }
}