using UnityEngine;

namespace Assets.Scripts.Enemy.UtilityAi
{
    public abstract class Consideration : ScriptableObject
    {
        public float Score
        {
            get => _score;
            set => _score = Mathf.Clamp01(value);
        }

        private float _score;

        public void Awake() => 
            Score = 0;

        public abstract float ScoreConsideration(EnemyBehavior behavior);
    }
}