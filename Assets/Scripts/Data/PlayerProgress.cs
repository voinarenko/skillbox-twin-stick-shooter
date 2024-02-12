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
        public PlayerDynamicData PlayerDynamicData;

        public PlayerProgress(PlayerStaticData playerStaticData)
        {
            WorldData = new WorldData();
            PlayerDynamicData = new PlayerDynamicData();
            PlayerStaticData = playerStaticData;
            PlayerState = new State();
        }
    }
}