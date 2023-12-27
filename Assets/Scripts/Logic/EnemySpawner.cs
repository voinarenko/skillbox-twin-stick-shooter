using Assets.Scripts.Data;
using Assets.Scripts.Enemy;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.StaticData;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        public EnemyTypeId EnemyTypeId;
        private string _id;

        [SerializeField] private bool _slain;
        public bool Slain => _slain;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
            _factory = AllServices.Container.Single<IGameFactory>();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(_id))
                _slain = true;
            else 
                Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if(Slain)
                progress.KillData.ClearedSpawners.Add(_id);
        }

        private void Spawn()
        {
            var enemy = _factory.CreateEnemy(EnemyTypeId, transform);
            _enemyDeath = enemy.GetComponent<EnemyDeath>();
            _enemyDeath.Happened += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null)
                _enemyDeath.Happened -= Slay;
            _slain = true;
        }
    }
}