using System;
using UnityEngine.Events;

namespace Assets.Scripts.Logic
{
    public interface IHealth
    {
        event Action HealthChanged;
        float Current { get; set; }
        float Max { get; set; }
        void RpcTakeDamage(float damage);
    }
}