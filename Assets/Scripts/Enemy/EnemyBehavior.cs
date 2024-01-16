using Assets.Scripts.Enemy.UtilityAi;
using Assets.Scripts.Enemy.UtilityAi.Actions;
using Assets.Scripts.Logic;
using UnityEngine;
using Action = Assets.Scripts.Enemy.UtilityAi.Action;

namespace Assets.Scripts.Enemy
{
    public class EnemyBehavior : MonoBehaviour
    {
        public EnemyMoveToPlayer Mover => GetComponent<EnemyMoveToPlayer>();
        public IHealth PlayerHealth => Mover.PlayerTransform.GetComponent<IHealth>();
        public Action[] ActionsAvailable;

        private AiBrain AiBrain => GetComponent<AiBrain>();
        public EnemyAttack Attacker => GetComponent<EnemyAttack>();

        private void Start()
        {
            Attacker.Completed += Completed;
            Mover.Completed += Completed;
            AiBrain.Decided += ExecuteAction;
            AiBrain.SetAction(ActionsAvailable[1]);
        }

        private void OnDestroy()
        {
            Attacker.Completed -= Completed;
            Mover.Completed -= Completed;
            AiBrain.Decided -= ExecuteAction;
        }

        public void DoMove()
        {
            Debug.Log($"{Attacker.Type} is moving");
            Attacker.DisableAttack();
            Mover.ApproachPlayer = true;
        }

        public void DoAttack()
        {
            Debug.Log($"{Attacker.Type} is attacking");
            Attacker.EnableAttack();
        }

        public void DoWait()
        {
            Debug.Log($"{Attacker.Type} is waiting");
            Attacker.DisableAttack();
        }

        private void ExecuteAction() => 
            AiBrain.BestAction.Execute(this);

        private void Completed() => 
            AiBrain.DecideBestAction(ActionsAvailable);
    }
}