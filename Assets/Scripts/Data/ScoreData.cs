using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class ScoreData
    {
        public int Score;

        public void UpdateScore(int score) => 
            Score += score;
    }
}