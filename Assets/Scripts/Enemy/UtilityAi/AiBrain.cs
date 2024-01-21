using UnityEngine;

namespace Assets.Scripts.Enemy.UtilityAi
{
    public class AiBrain : MonoBehaviour
    {
        public Action BestAction;// { get; private set; }
        public event System.Action Decided;

        private EnemyBehavior _enemy;

        private void Start() =>
            _enemy = GetComponent<EnemyBehavior>();

        public void SetAction(Action action)
        {
            BestAction = action;
            Decided?.Invoke();
        }

        public void DecideBestAction(Action[] actionsAvailable)
        {
            var score = 0f;
            var nextBestActionIndex = 0;
            for (var i = 0; i < actionsAvailable.Length; i++)
            {
                if (!(ScoreAction(actionsAvailable[i]) > score)) continue;
                nextBestActionIndex = i;
                score = actionsAvailable[i].Score;
            }

            BestAction = actionsAvailable[nextBestActionIndex];
            Decided?.Invoke();
        }

        private float ScoreAction(Action action)
        {
            var score = 1f;
            foreach (var consideration in action.Considerations)
            {
                var considerationScore = consideration.ScoreConsideration(_enemy);
                score *= considerationScore;

                if (score != 0f) continue;
                action.Score = 0;
                return action.Score;
            }
            var originalScore = score;
            var modFactor = 1 - (1 / action.Considerations.Length);
            var makeupValue = (1 - originalScore) * modFactor;
            action.Score = originalScore + (makeupValue * originalScore);

            return action.Score;
        }
    }
}