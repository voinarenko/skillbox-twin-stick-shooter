using Assets.Scripts.Data;
using Assets.Scripts.Enemy.UtilityAi;
using Mirror;
using System.Collections;
using Assets.Scripts.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Action = System.Action;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator), typeof(NavMeshAgent))]
    public class EnemyDeath : NetworkBehaviour
    {
        public event Action Happened;
        [SyncVar] public int Value;
        [SerializeField] private GameObject _deathFx;
        [SerializeField] private TextMeshPro _lootText;
        [SerializeField] private GameObject _pickupPopup;

        private const float TimeToDestroy = 3;
        private const float TimeToSpawnLoot = 2.5f;
        private PlayerProgress _progress;
        private PlayersWatcher _watcher;
        private EnemyMoveToPlayer _mover;
        private EnemyHealth _health;
        private EnemyAnimator _animator;
        private NavMeshAgent _agent;
        private AiBrain _aiBrain;
        private EnemyAttack _attack;
        private EnemyBehavior _behavior;
        private BoxCollider _collider;

        public void Construct(PlayerProgress progress, PlayersWatcher watcher)
        {
            _progress = progress;
            _watcher = watcher;
        }

        private void Start()
        {
            _mover = GetComponent<EnemyMoveToPlayer>();
            _health = GetComponent<EnemyHealth>();
            _animator = GetComponent<EnemyAnimator>();
            _agent = GetComponent<NavMeshAgent>();
            _aiBrain = GetComponent<AiBrain>();
            _attack = GetComponent<EnemyAttack>();
            _behavior = GetComponent<EnemyBehavior>();
            _collider = GetComponentInChildren<BoxCollider>();

            _health.HealthChanged += HealthChanged;
        }

        private void HealthChanged()
        {
            if (_health.Current <= 0)
                RpcDie();
        }

        [ClientRpc]
        private void RpcDie()
        {
            CmdUpdateGlobalData();
            CmdUpdateScore();
            _collider.enabled = false;
            _health.HealthChanged -= HealthChanged;
            _aiBrain.SetAction(_behavior.ActionsAvailable[2]);
            _mover.enabled = false;
            _aiBrain.enabled = false;
            _agent.updatePosition = false;
            _agent.updateRotation = false;
            _agent.speed = 0;
            _attack.enabled = false;
            _animator.PlayDeath();
            CmdSpawnDeathFx();
            CmdShowText();
            StartCoroutine(Inform());
            Destroy(gameObject, TimeToDestroy);
        }

        [Command(requiresAuthority = false)]
        private void CmdUpdateScore() => 
            _watcher.UpdateScore(Value);

        [Command(requiresAuthority = false)]
        private void CmdUpdateGlobalData()
        {
            _progress.WorldData.WaveData.RemoveEnemy();
            _progress.WorldData.KillData.Collect(_attack);
        }

        [Command(requiresAuthority = false)]
        private void CmdShowText()
        {
            _lootText.text = $"{Value}";
            _pickupPopup.SetActive(true);
        }

        private IEnumerator Inform()
        {
            yield return new WaitForSeconds(TimeToSpawnLoot);
            Happened?.Invoke();
        }

        [Command(requiresAuthority = false)]
        private void CmdSpawnDeathFx()
        {
            var effect = Instantiate(_deathFx, transform.position, Quaternion.identity);
            NetworkServer.Spawn(effect);
        }

#pragma warning disable IDE0051
        private void OnDeath() => 
            _agent.isStopped = true;
#pragma warning restore IDE0051
    }
}