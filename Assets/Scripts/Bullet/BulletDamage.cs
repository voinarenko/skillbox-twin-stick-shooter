using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.Bullet
{
    public class BulletDamage : MonoBehaviour
    {
        public float Damage;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.transform.CompareTag("Enemy")) return;
            other.transform.parent.GetComponent<IHealth>().TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}