using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Static Data/Player")]
    public class PlayerStaticData : ScriptableObject
    {
        public PlayerTypeId PlayerTypeId;

        [Range(50, 200)] public int Health;
        [Range(5, 50)] public float Damage;
        [Range(25, 100)] public int Ammo;

        public float SpeedFactor;

        [Range(0.0025f, 0.01f)] public float MoveSpeed;
        [Range(0, 2000)] public float RotateSpeed;

        [Range(0f, 0.6f)] public float AttackCooldown;
        [Range(0, 3)] public float ReloadCooldown;

        public Sprite Image;
        public AssetReferenceGameObject PrefabReference;
    }
}