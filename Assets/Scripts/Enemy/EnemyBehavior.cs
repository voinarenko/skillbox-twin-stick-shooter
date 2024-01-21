using Assets.Scripts.Enemy.UtilityAi;
using Assets.Scripts.Logic;
using UnityEngine;
using UnityEngine.AI;
using Action = Assets.Scripts.Enemy.UtilityAi.Action;

namespace Assets.Scripts.Enemy
{
    public class EnemyBehavior : MonoBehaviour
    {
        public EnemyMoveToPlayer Mover;
        public IHealth PlayerHealth;
        public Action[] ActionsAvailable;

        private NavMeshAgent _agent;
        private AiBrain _aiBrain;
        private EnemyAttack _attacker;

        private void Start()
        {
            Mover = GetComponent<EnemyMoveToPlayer>();
            PlayerHealth = Mover.PlayerTransform.GetComponent<IHealth>();
            _aiBrain = GetComponent<AiBrain>();
            _attacker = GetComponent<EnemyAttack>();
            _agent = GetComponent<NavMeshAgent>();
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
            _agent.isStopped = false;
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