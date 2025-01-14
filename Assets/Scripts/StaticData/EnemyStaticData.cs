﻿using UnityEngine;
using UnityEngine.AddressableAssets;

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

        public float BoostFactor = 0.1f;

        [Range(1, 10)]
        public float MoveSpeed;
        [Range(1, 2000)]
        public float RotateSpeed;
        [Range(1,204
            )]
        public float Acceleration;
        [Range(1, 6)]
        public float StoppingDistance;

        [Range(0.5f, 1)]
        public float Cleavage;
        [Range(1, 5)]
        public float AttackCooldown;

        public AssetReferenceGameObject PrefabReference;
        
        public int KillValue;
    }
}