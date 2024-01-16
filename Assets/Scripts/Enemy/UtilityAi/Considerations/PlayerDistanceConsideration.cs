using UnityEngine;

namespace Assets.Scripts.Enemy.UtilityAi.Considerations
{
    [CreateAssetMenu(fileName = "PlayerDistanceConsideration", menuName = "UtilityAI/Considerations/Distance Consideration")]
    public class PlayerDistanceConsideration : Consideration
    {
        public override float ScoreConsideration(EnemyBehavior behavior) =>
            behavior.Mover.Agent.remainingDistance > behavior.Mover.Agent.stoppingDistance  ? 1 : 0;
    }
}