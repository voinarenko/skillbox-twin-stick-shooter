using Assets.Scripts.Data;
using Assets.Scripts.Logic;
using Mirror;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerAnimator), typeof(PlayerHudConnector))]
    public class PlayerHealth : NetworkBehaviour, IHealth
    {
        public event Action HealthChanged;

        public float Max
        {
            get => _state.MaxHealth;
            set => _state.MaxHealth = value;
        }

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

        public float Defense = 1;

        private readonly State _state = new();

        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private PlayerHudConnector _hudConnector;
        
        [ClientRpc]
        public void RpcSetHealth(float maxHealth)
        {
            _state.MaxHealth = maxHealth;
            _state.ResetHealth();
            _hudConnector.PlayerHealth = maxHealth;
        }

        [ClientRpc]
        public void RpcTakeDamage(float damage)
        {
            if (Current <= 0) return;
            Current -= damage / Defense;
            _animator.PlayHit();
        }

        [ClientRpc]
        public void RpcHeal(float cure)
        {
            Current += cure;
            if (Current > Max) Current = Max;
        }
    }
}