using UnityEngine;

namespace Assets.Scripts.Enemy.UtilityAi.Actions
{
    [CreateAssetMenu(fileName = "Move", menuName = "UtilityAI/Actions/Move")]
    public class Move : Action
    {
        public override void Execute(EnemyBehavior enemy) => 
            enemy.DoMove();
    }
}