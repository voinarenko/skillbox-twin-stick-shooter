using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public AmmoData AmmoData;
        public ConsumableData ConsumableData;
        public PerkData PerkData;
        public WaveData WaveData;
        public KillData KillData;
        public ScoreData ScoreData;
        public SpentData SpentData;

        public WorldData(string initialLevel)
        {
            PositionOnLevel = new PositionOnLevel(initialLevel);
            AmmoData = new AmmoData();
            ConsumableData = new ConsumableData();
            PerkData = new PerkData();
            WaveData = new WaveData();
            KillData = new KillData();
            ScoreData = new ScoreData();
            SpentData = new SpentData();
        }
    }
}