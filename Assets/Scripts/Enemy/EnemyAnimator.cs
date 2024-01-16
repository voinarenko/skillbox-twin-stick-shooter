using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        private Animator _animator;

        private void Awake() => 
            _animator = GetComponent<Animator>();

        public void PlayDeath() => _animator.SetTrigger(Die);
        public void PlayHit() => _animator.SetTrigger(Hit);
        public void Move() => _animator.SetBool(IsMoving, true);
        public void StopMoving() => _animator.SetBool(IsMoving, false);
        public void PlayAttack() => _animator.SetTrigger(Attack);
        public void PlayShoot() => _animator.SetTrigger(Shoot);
    }
}