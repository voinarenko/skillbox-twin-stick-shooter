using Assets.Scripts.Enemy;
using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class ScoreData
    {
        public int Score;

        public void UpdateScore(EnemyDeath enemy) => 
            Score += enemy.Value;
    }
}