using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Logic;
using Mirror;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerAnimator))]
    public class PlayerHealth : NetworkBehaviour, ISavedProgress, IHealth
    {
        public float Max
        {
            get => _state.MaxHealth;
            set => _state.MaxHealth = value;
        }

        public event Action HealthChanged;

        public float Current
        {
            get => _state.CurrentHealth;
            set
            {
                _state.CurrentHealth = value;
                HealthChanged?.Invoke();
            }
        }

        public PlayerAnimator Animator;
        public float Defense = 1;

        private State _state;

        //public void LoadProgress(PlayerProgress progress)
        //{
        //    _state = progress.PlayerState;
        //    HealthChanged?.Invoke();
        //}

        //public void UpdateProgress(PlayerProgress progress)
        //{
        //    progress.PlayerState.CurrentHealth = Current;
        //    progress.PlayerState.MaxHealth = Max;
        //}

        public void TakeDamage(float damage)
        {
            if (Current <= 0) return;
            Current -= damage / Defense;
            Animator.PlayHit();
        }

        public void Heal(float cure)
        {
            Current += cure;
            if (Current > Max) Current = Max;
        }
    }
}