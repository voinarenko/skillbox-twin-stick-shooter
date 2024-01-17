using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.Bullet
{
    public class BulletDamage : MonoBehaviour
    {
        public GameObject HitFxPrefab;
        public string Sender;
        public float Damage;

        private const string PlayerTag = "Player";
        private const string EnemyTag = "Enemy";

        private bool _collided;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Sender)) return;
            if (!other.transform.CompareTag(EnemyTag) && !other.transform.CompareTag(PlayerTag)) return;
            if (_collided) return;
            _collided = true;
            other.transform.parent.GetComponent<IHealth>().TakeDamage(Damage);
            Instantiate(HitFxPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}