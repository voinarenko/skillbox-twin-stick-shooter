using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.Bullet
{
    public class BulletDamage : MonoBehaviour
    {
        public string Sender;
        public float Damage;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Sender)) return;
            if (!other.transform.CompareTag("Enemy") && !other.transform.CompareTag("Player")) return;
            other.transform.parent.GetComponent<IHealth>().TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}