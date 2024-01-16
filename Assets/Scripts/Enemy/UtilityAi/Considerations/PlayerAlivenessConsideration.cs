using UnityEngine;

namespace Assets.Scripts.Enemy.UtilityAi.Considerations
{
    [CreateAssetMenu(fileName = "PlayerAlivenessConsideration", menuName = "UtilityAI/Considerations/Aliveness Consideration")]
    public class PlayerAlivenessConsideration : Consideration
    {
        public override float ScoreConsideration(EnemyBehavior behavior) => 
            behavior.PlayerHealth.Current <= 0 ? 1 : 0;
    }
}