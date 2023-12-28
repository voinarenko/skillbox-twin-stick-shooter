using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.StaticData;
using Assets.Scripts.StaticData.Windows;
using Assets.Scripts.UI.Services.Windows;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataEnemiesPath = "StaticData/Enemies";
        private const string StaticDataLevelsPath = "StaticData/Levels";
        private const string StaticDataWindowsPath = "StaticData/UI/WindowStaticData";
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;

        public void Load()
        {
            _enemies = Resources
                .LoadAll<EnemyStaticData>(StaticDataEnemiesPath)
                .ToDictionary(x => x.EnemyTypeId, x => x);
            _levels = Resources
                .LoadAll<LevelStaticData>(StaticDataLevelsPath)
                .ToDictionary(x => x.LevelKey, x => x);
            _windowConfigs = Resources
                .Load<WindowStaticData>(StaticDataWindowsPath)
                .Configs
                .ToDictionary(x => x.WindowId, x => x);
        }

        public EnemyStaticData ForEnemy(EnemyTypeId typeId) => 
            _enemies.TryGetValue(typeId, out var enemyStaticData) 
                ? enemyStaticData 
                : null;

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.TryGetValue(sceneKey, out var levelStaticData)
                ? levelStaticData
                : null;

        public WindowConfig ForWindow(WindowId windowId) =>
            _windowConfigs.TryGetValue(windowId, out var windowConfig)
                ? windowConfig
                : null;
    }
}