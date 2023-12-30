using Assets.Scripts.Data;
using Assets.Scripts.Enemy;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.StaticData;
using UnityEngine;

namespace Assets.Scripts.Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public EnemyTypeId EnemyTypeId;
        public string Id { get; set; }

        private bool _slain;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        public void Construct(IGameFactory factory) =>
            _factory = factory;

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(Id))
                _slain = true;
            else 
                Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if(_slain)
                progress.KillData.ClearedSpawners.Add(Id);
        }

        private async void Spawn()
        {
            var enemy = await _factory.CreateEnemy(EnemyTypeId, transform);
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