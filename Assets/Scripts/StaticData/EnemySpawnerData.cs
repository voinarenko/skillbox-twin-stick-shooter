using System;
using UnityEngine;

namespace Assets.Scripts.StaticData
{
    [Serializable]
    public class EnemySpawnerData
    {
        public string Id;
        public Vector3 Position;

        public EnemySpawnerData(string id, Vector3 position)
        {
            Id = id;
            Position = position;
        }
    }
}