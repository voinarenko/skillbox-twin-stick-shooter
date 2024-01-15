using System;
using Assets.Scripts.StaticData;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public PlayerStaticData PlayerStaticData;
        public State PlayerState;
        public WorldData WorldData;
        public Stats PlayerStats;
        public KillData KillData;

        public PlayerProgress(string initialLevel, PlayerStaticData playerStaticData)
        {
            WorldData = new WorldData(initialLevel);
            PlayerStaticData = playerStaticData;
            PlayerState = new State();
            PlayerStats = new Stats();
            KillData = new KillData();
        }
    }
}