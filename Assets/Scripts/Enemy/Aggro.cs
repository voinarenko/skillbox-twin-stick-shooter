using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Aggro : MonoBehaviour
    {
        public TriggerObserver TriggerObserver;
        public EnemyMoveToPlayer Follow;

        public float Cooldown;
        private Coroutine _aggroCoroutine;
        private bool _hasAggroTarget;

        private void Start()
        {
            TriggerObserver.TriggerEnter += TriggerEnter;
            TriggerObserver.TriggerExit += TriggerExit;

            SwitchFollow(false);
        }

        private void TriggerEnter(Collider other)
        {
            if (_hasAggroTarget) return;
            _hasAggroTarget = true;
            StopAggroCoroutine();
            SwitchFollow(true);
        }

        private void TriggerExit(Collider other)
        {
            if (!_hasAggroTarget) return;
            _hasAggroTarget = false; 
            _aggroCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown());
        }

        private void StopAggroCoroutine()
        {
            if (_aggroCoroutine == null) return;
            StopCoroutine(_aggroCoroutine);
            _aggroCoroutine = null;
        }

        private IEnumerator SwitchFollowOffAfterCooldown()
        {
            yield return new WaitForSeconds(Cooldown);
            SwitchFollow(false);
        }

        private void SwitchFollow(bool value) => 
            Follow.enabled = value;
    }
}