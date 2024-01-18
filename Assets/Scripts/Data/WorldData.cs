using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public AmmoData AmmoData;
        public LootData LootData;
        public WaveData WaveData;

        public WorldData(string initialLevel)
        {
            PositionOnLevel = new PositionOnLevel(initialLevel);
            AmmoData = new AmmoData();
            LootData = new LootData();
            WaveData = new WaveData();
        }
    }
}