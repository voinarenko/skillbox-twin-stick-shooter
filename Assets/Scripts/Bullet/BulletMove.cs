using System.Collections;
using Mirror;
using UnityEngine;

namespace Assets.Scripts.Bullet
{
    public class BulletMove : NetworkBehaviour
    {
        private const string DoDisableMethodName = "DoDisable";
        private const float TimeToDestroy = 3;

        [SerializeField] private BulletTrailScriptableObject _trailConfig;
        [SerializeField] private Renderer _renderer;

        [SerializeField] private float _speed;

        private TrailRenderer _trail;


        protected virtual void OnEnable()
        {
            _renderer.enabled = true;
            ConfigureTrail();
            StartCoroutine(DestroyTimer());
        }

        private void Awake() => 
            _trail = GetComponent<TrailRenderer>();

        private void Update()
        {
            if (transform == null) return; 
            transform.position += transform.forward * _speed;
        }

        protected void Disable()
        {
            CancelInvoke(DoDisableMethodName);
            _renderer.enabled = false;
            if (_trail != null && _trailConfig != null)
                Invoke(DoDisableMethodName, _trailConfig.Time);
            else
                DoDisable();
        }

        private void DoDisable()
        {
            if (_trail != null && _trailConfig != null) 
                _trail.Clear();
            gameObject.SetActive(false);
        }

        private void ConfigureTrail()
        {
            if (_trail != null && _trailConfig != null) 
                _trailConfig.SetupTrail(_trail);
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(TimeToDestroy);
            DestroySelf();
        }

        [Server]
        private void DestroySelf() => 
            NetworkServer.Destroy(gameObject);
    }
}
