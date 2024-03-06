using Assets.Scripts.StaticData;
using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class State
    {
        public PlayerTypeId CurrentType;
        public float CurrentHealth;
        public float MaxHealth;

        public void ResetHealth() => CurrentHealth = MaxHealth;
    }
}