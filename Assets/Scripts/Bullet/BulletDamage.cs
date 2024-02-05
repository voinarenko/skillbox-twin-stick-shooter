using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.Bullet
{
    public class BulletDamage : MonoBehaviour
    {
        public float Damage;
        public string Sender;

        private const string PlayerTag = "Player";
        private const string EnemyTag = "Enemy";
        private const string WallTag = "Wall";

        [SerializeField] private GameObject _hitFxPrefab;

        private bool _collided;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(WallTag)) Destroy(gameObject);
            if (other.CompareTag(Sender)) return;
            if (!other.transform.CompareTag(EnemyTag) && !other.transform.CompareTag(PlayerTag)) return;
            if (_collided) return;
            _collided = true;
            other.transform.parent.GetComponent<IHealth>().TakeDamage(Damage);
            Instantiate(_hitFxPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}