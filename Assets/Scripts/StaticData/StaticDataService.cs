using Assets.Scripts.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private Dictionary<string, LevelStaticData> _levels;

        public void Load()
        {
            _enemies = Resources
                .LoadAll<EnemyStaticData>("StaticData/Enemies")
                .ToDictionary(x => x.EnemyTypeId, x => x);
            _levels = Resources
                .LoadAll<LevelStaticData>("StaticData/Levels")
                .ToDictionary(x => x.LevelKey, x => x);
        }

        public EnemyStaticData ForEnemy(EnemyTypeId typeId) => 
            _enemies.TryGetValue(typeId, out var staticData) 
                ? staticData 
                : null;

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.TryGetValue(sceneKey, out var staticData)
                ? staticData
                : null;
    }
}