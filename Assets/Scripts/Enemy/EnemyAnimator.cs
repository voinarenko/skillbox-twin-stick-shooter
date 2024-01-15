using System;
using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
    {
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        private readonly int _idleStateHash = Animator.StringToHash("mutant idle");
        private readonly int _attackStateHash = Animator.StringToHash("mutant swiping");
        private readonly int _walkingStateHash = Animator.StringToHash("mutant walking");
        private readonly int _deathStateHash = Animator.StringToHash("mutant dying");

        private Animator _animator;

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public AnimatorState State { get; private set; }

        private void Awake() => 
            _animator = GetComponent<Animator>();

        public void PlayDeath() => _animator.SetTrigger(Die);
        public void PlayHit() => _animator.SetTrigger(Hit);
        public void Move() => _animator.SetBool(IsMoving, true);
        public void StopMoving() => _animator.SetBool(IsMoving, false);
        public void PlayAttack() => _animator.SetTrigger(Attack);
        public void PlayShoot() => _animator.SetTrigger(Shoot);

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash) => 
            StateExited?.Invoke(StateFor(stateHash));

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _attackStateHash)
                state = AnimatorState.Attack;
            else if (stateHash == _walkingStateHash)
                state = AnimatorState.Walking;
            else if (stateHash == _deathStateHash)
                state = AnimatorState.Died;
            else
                state = AnimatorState.Unknown;

            return state;
        }
    }
}