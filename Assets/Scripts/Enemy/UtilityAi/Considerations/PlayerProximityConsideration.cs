using UnityEngine;

namespace Assets.Scripts.Enemy.UtilityAi.Considerations
{
    [CreateAssetMenu(fileName = "PlayerProximityConsideration", menuName = "UtilityAI/Considerations/Proximity Consideration")]
    public class PlayerProximityConsideration : Consideration
    {
        public override float ScoreConsideration(EnemyBehavior behavior) =>
            behavior.Mover.Agent.remainingDistance <= behavior.Mover.Agent.stoppingDistance &&
            behavior.Mover.Agent.remainingDistance != 0 &&
            behavior.PlayerHealth.Current > 0
                ? 1
                : 0;
    }
}