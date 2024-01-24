using Assets.Scripts.StaticData;
using Assets.Scripts.StaticData.Windows;
using Assets.Scripts.UI.Services.Windows;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataPlayerPath = "StaticData/Player";
        private const string StaticDataEnemiesPath = "StaticData/Enemies";
        private const string StaticDataLevelsPath = "StaticData/Levels";
        private const string StaticDataConsumablesPath = "StaticData/Consumables";
        private const string StaticDataPerksPath = "StaticData/Perks";
        private const string StaticDataWindowsPath = "StaticData/UI/WindowStaticData";
        private Dictionary<PlayerTypeId, PlayerStaticData> _players;
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<ConsumableTypeId, ConsumableStaticData> _consumables;
        private Dictionary<PerkTypeId, PerkStaticData> _perks;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;
        
        public void Load()
        {
            _players = Resources
                .LoadAll<PlayerStaticData>(StaticDataPlayerPath)
                .ToDictionary(x => x.PlayerTypeId, x => x);
            _enemies = Resources
                .LoadAll<EnemyStaticData>(StaticDataEnemiesPath)
                .ToDictionary(x => x.EnemyTypeId, x => x);
            _levels = Resources
                .LoadAll<LevelStaticData>(StaticDataLevelsPath)
                .ToDictionary(x => x.LevelKey, x => x);
            _consumables = Resources
                .LoadAll<ConsumableStaticData>(StaticDataConsumablesPath)
                .ToDictionary(x => x.LootTypeId, x => x);
            _perks = Resources
                .LoadAll<PerkStaticData>(StaticDataPerksPath)
                .ToDictionary(x => x.LootTypeId, x => x);
            _windowConfigs = Resources
                .Load<WindowStaticData>(StaticDataWindowsPath)
                .Configs
                .ToDictionary(x => x.WindowId, x => x);
        }

        public PlayerStaticData ForPlayer(PlayerTypeId typeId) => 
            _players.GetValueOrDefault(typeId);

        public EnemyStaticData ForEnemy(EnemyTypeId typeId) => 
            _enemies.GetValueOrDefault(typeId);

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.GetValueOrDefault(sceneKey);

        public WindowConfig ForWindow(WindowId windowId) =>
            _windowConfigs.GetValueOrDefault(windowId);

        public ConsumableStaticData ForConsumable(ConsumableTypeId typeId) => 
            _consumables.GetValueOrDefault(typeId);

        public PerkStaticData ForPerk(PerkTypeId typeId) =>
            _perks.GetValueOrDefault(typeId);
    }
}