using System;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(Collider))]
    public class TriggerObserver : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        public event Action<Collider> TriggerEnter;
        public event Action<Collider> TriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag)) 
                TriggerEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(PlayerTag)) 
                TriggerExit?.Invoke(other);
        }
    }
}