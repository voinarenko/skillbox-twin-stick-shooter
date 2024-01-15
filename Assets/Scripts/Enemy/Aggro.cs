using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Aggro : MonoBehaviour
    {
        public TriggerObserver TriggerObserver;
        public EnemyMoveToPlayer Follow;

        private void Start()
        {
            TriggerObserver.TriggerEnter += TriggerEnter;
            TriggerObserver.TriggerExit += TriggerExit;

            SwitchFollow(true);
        }

        private void OnDestroy()
        {
            TriggerObserver.TriggerEnter -= TriggerEnter;
            TriggerObserver.TriggerExit -= TriggerExit;
        }

        private void TriggerEnter(Collider other) => 
            Follow.PlayerNearby = true;

        private void TriggerExit(Collider other) => 
            Follow.PlayerNearby = false;

        private void SwitchFollow(bool value) => 
            Follow.enabled = value;
    }
}