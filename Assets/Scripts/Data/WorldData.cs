using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class WorldData
    {
        public WaveData WaveData = new();
        public KillData KillData = new();
    }
}