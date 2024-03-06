using Assets.Scripts.StaticData;
using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public PlayerStaticData PlayerStaticData;
        public State PlayerState;
        public WorldData WorldData;

        public PlayerProgress(PlayerStaticData playerStaticData)
        {
            WorldData = new WorldData();
            PlayerStaticData = playerStaticData;
            PlayerState = new State();
        }
    }
}