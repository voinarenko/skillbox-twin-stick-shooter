using System;
using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerAnimation))]
    public class PlayerHealth : MonoBehaviour, ISavedProgress, IHealth
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
                if (Math.Abs(_state.CurrentHealth - value) > 0.01f)
                {
                    _state.CurrentHealth = value;
                    HealthChanged?.Invoke();
                }
            }
        }

        public PlayerAnimation Animator;

        private State _state;

        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.PlayerState;
            Debug.Log(_state);
            HealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.PlayerState.CurrentHealth = Current;
            progress.PlayerState.MaxHealth = Max;
        }

        public void TakeDamage(float damage)
        {
            if (Current <= 0) return;
            Current -= damage;
            Animator.PlayHit();
        }
    }
}