using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Logic;
using Mirror;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerAnimator), typeof(PlayerHudConnector))]
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
                _hudConnector.PlayerHealth = value;
            }
        }

        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private PlayerHudConnector _hudConnector;
        public float Defense = 1;

        private readonly State _state = new();

        [ClientRpc]
        public void RpcSetHealth(float maxHealth)
        {
            print($"Health: |{maxHealth}|");
            _state.MaxHealth = maxHealth;
            _state.ResetHealth();
            _hudConnector.PlayerHealth = maxHealth;
        }

        //public void UpdateProgress(PlayerProgress progress)
        //{
        //    progress.PlayerState.CurrentHealth = Current;
        //    progress.PlayerState.MaxHealth = Max;
        //}

        public void TakeDamage(float damage)
        {
            if (Current <= 0) return;
            Current -= damage / Defense;
            _animator.PlayHit();
        }

        public void Heal(float cure)
        {
            Current += cure;
            if (Current > Max) Current = Max;
        }
    }
}