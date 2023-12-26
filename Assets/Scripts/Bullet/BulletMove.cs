using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Bullet
{
    public class BulletMove : MonoBehaviour
    {
        public BulletTrailScriptableObject TrailConfig;

        private const string DoDisableMethodName = "DoDisable";
        private TrailRenderer _trail;

#pragma warning disable CS0649
        [SerializeField] private float _speed;
        [SerializeField] private Renderer _renderer;
#pragma warning restore CS0649

        protected virtual void OnEnable()
        {
            _renderer.enabled = true;
            ConfigureTrail();
            StartCoroutine(DestroyTimer());
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
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
            if (_trail != null && TrailConfig != null)
                Invoke(DoDisableMethodName, TrailConfig.Time);
            else
                DoDisable();
        }

        private void DoDisable()
        {
            if (_trail != null && TrailConfig != null) 
                _trail.Clear();
            gameObject.SetActive(false);
        }

        private void ConfigureTrail()
        {
            if (_trail != null && TrailConfig != null) 
                TrailConfig.SetupTrail(_trail);
        }
    }
}
