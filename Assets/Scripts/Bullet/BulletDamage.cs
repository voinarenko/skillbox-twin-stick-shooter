using Assets.Scripts.Logic;
using Mirror;
using UnityEngine;

namespace Assets.Scripts.Bullet
{
    public class BulletDamage : NetworkBehaviour
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
            if (!isServer) return;
            if (other.CompareTag(WallTag)) DestroySelf();
            if (other.CompareTag(Sender)) return;
            if (!other.transform.CompareTag(EnemyTag) && !other.transform.CompareTag(PlayerTag)) return;
            if (_collided) return;
            _collided = true;
            other.transform.parent.GetComponent<IHealth>().RpcTakeDamage(Damage);
            CmdHit();
            DestroySelf();
        }

        [Server]
        private void CmdHit()
        {
            var effect = Instantiate(_hitFxPrefab, transform.position, Quaternion.identity);
            NetworkServer.Spawn(effect);
        }

        [Server]
        private void DestroySelf() => 
            NetworkServer.Destroy(gameObject);
    }
}