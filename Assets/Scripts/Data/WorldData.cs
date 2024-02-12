using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class WorldData
    {
        public ConsumableData ConsumableData = new();
        public PerkData PerkData = new();
        public WaveData WaveData = new();
        public KillData KillData = new();
    }
}