using Assets.Scripts.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;

        public void LoadEnemies() => 
            _enemies = Resources
                .LoadAll<EnemyStaticData>("Enemies/Static Data")
                .ToDictionary(x => x.EnemyTypeId, x => x);

        public EnemyStaticData ForEnemy(EnemyTypeId typeId) => 
            _enemies.TryGetValue(typeId, out var staticData) 
                ? staticData 
                : null;
    }
}