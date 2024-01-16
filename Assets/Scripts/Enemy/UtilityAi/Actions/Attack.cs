using UnityEngine;

namespace Assets.Scripts.Enemy.UtilityAi.Actions
{
    [CreateAssetMenu(fileName = "EnemyAttack", menuName = "UtilityAI/Actions/EnemyAttack")]
    public class Attack : Action
    {
        public override void Execute(EnemyBehavior enemy) => 
            enemy.DoAttack();
    }
}