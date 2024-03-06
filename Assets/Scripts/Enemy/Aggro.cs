using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Aggro : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private EnemyMoveToPlayer _follow;

        private void Start()
        {
            _triggerObserver.TriggerEnter += TriggerEnter;
            _triggerObserver.TriggerExit += TriggerExit;

            SwitchFollow(true);
        }

        private void OnDestroy()
        {
            _triggerObserver.TriggerEnter -= TriggerEnter;
            _triggerObserver.TriggerExit -= TriggerExit;
        }

        private void TriggerEnter(Collider other)
        {
            _follow.InitTarget(other.GetComponentInParent<PlayerHealth>().gameObject);
            _follow.PlayerNearby = true;
        }

        private void TriggerExit(Collider other) => 
            _follow.PlayerNearby = false;

        private void SwitchFollow(bool value) => 
            _follow.enabled = value;
    }
}