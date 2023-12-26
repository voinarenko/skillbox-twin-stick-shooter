using UnityEngine;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Static Data/Enemy")]
    public class EnemyStaticData : ScriptableObject
    {
        public EnemyTypeId EnemyTypeId;

        [Range(1, 100)]
        public int Health;
        [Range(1, 30)]
        public float Damage;

        [Range(1, 5)]
        public float EffectiveDistance;
        [Range(0.5f, 1)]
        public float Cleavage;

        public GameObject Prefab;
        public float MoveSpeed;
    }
}