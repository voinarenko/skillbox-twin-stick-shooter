﻿using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public State PlayerState;
        public WorldData WorldData;
        public Stats PlayerStats;
        public KillData KillData;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            PlayerState = new State();
            PlayerStats = new Stats();
            KillData = new KillData();
        }
    }
}