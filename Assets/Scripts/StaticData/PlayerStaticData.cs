using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Static Data/Player")]
    public class PlayerStaticData : ScriptableObject
    {
        public PlayerTypeId PlayerTypeId;

        [Range(100, 200)] public int Health;
        [Range(20, 50)] public float Damage;
        [Range(50, 100)] public int Ammo;

        [Range(0.005f, 0.01f)] public float MoveSpeed;
        [Range(500, 2000)] public float RotateSpeed;

        [Range(0.2f, 0.6f)] public float AttackCooldown;
        [Range(1, 3)] public float ReloadCooldown;

        public Sprite Image;
        public AssetReferenceGameObject PrefabReference;
    }
}