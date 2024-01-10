using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Static Data/Player")]
    public class PlayerStaticData : ScriptableObject
    {
        public PlayerTypeId PlayerTypeId;

        [Range(1, 200)]
        public int Health;
        [Range(1, 50)]
        public float Damage;

        [Range(0.005f, 0.1f)]
        public float MoveSpeed;
        [Range(1, 2000)]
        public float RotateSpeed;

        [Range(0, 1)]
        public float AttackCooldown;
        [Range(1, 3)]
        public float ReloadCooldown;

        public AssetReferenceGameObject PrefabReference;
    }
}