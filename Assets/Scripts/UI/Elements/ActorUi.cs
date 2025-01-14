﻿using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.UI.Elements
{
    public class ActorUi : MonoBehaviour
    {
        public HealthBar HealthBar;

        private IHealth _health;

        public void Construct(IHealth health)
        {
            _health = health;
            _health.HealthChanged += UpdateHealthBar;
        }

        private void Start()
        {
            if(TryGetComponent<IHealth>(out var health))
                Construct(health);
        }

        private void OnDestroy()
        {
            if (_health != null) 
                _health.HealthChanged -= UpdateHealthBar;
        }

        public void UpdateHealthBar(float currentHealth, float maxHealth) => 
            HealthBar.SetValue(currentHealth, maxHealth);

        private void UpdateHealthBar() => 
            HealthBar.SetValue(_health.Current, _health.Max);
    }
}