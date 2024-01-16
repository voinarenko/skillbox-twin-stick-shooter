using UnityEngine;

namespace Assets.Scripts.Enemy.UtilityAi.Actions
{
    [CreateAssetMenu(fileName = "Wait", menuName = "UtilityAI/Actions/Wait")]
    public class Wait : Action
    {
        public override void Execute(EnemyBehavior enemy) => 
            enemy.DoWait();
    }
}