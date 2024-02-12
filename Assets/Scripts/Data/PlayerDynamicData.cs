using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PlayerDynamicData
    {
        //public PositionOnLevel PositionOnLevel = new();
        public AmmoData AmmoData = new();
        public ScoreData ScoreData = new();
        public SpentData SpentData = new();
    }
}