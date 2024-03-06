using Assets.Scripts.Enemy.UtilityAi;
using Assets.Scripts.Logic;
using UnityEngine;
using Action = Assets.Scripts.Enemy.UtilityAi.Action;

namespace Assets.Scripts.Enemy
{
    public class EnemyBehavior : MonoBehaviour
    {
        public EnemyMoveToPlayer Mover { get; private set; }
        public IHealth PlayerHealth { get; set; }
        public Action[] ActionsAvailable;

        [SerializeField] private AiBrain _aiBrain;
        [SerializeField] private EnemyAttack _attacker;

        private void Start()
        {
            Mover = GetComponent<EnemyMoveToPlayer>();
            _attacker.Completed += Completed;
            Mover.Completed += Completed;
            _aiBrain.Decided += ExecuteAction;
            _aiBrain.SetAction(ActionsAvailable[1]);
        }

        private void OnDestroy()
        {
            _attacker.Completed -= Completed;
            Mover.Completed -= Completed;
            _aiBrain.Decided -= ExecuteAction;
        }

        public void DoMove()
        {
            Mover.SetDestinationForAgent();
            _attacker.DisableAttack();
        }

        public void DoAttack() => 
            _attacker.EnableAttack();

        public void DoWait() => 
            _attacker.DisableAttack();

        private void ExecuteAction() => 
            _aiBrain.BestAction.Execute(this);

        private void Completed() => 
            _aiBrain.DecideBestAction(ActionsAvailable);
    }
}