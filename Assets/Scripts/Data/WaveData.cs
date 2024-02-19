using Assets.Scripts.Infrastructure;
using System;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class WaveData
    {
        public int Encountered;
        public event Action EnemyRemoved;

        private const string WaveChangerTag = "WaveChanger";
        private int _currentEnemies;

        public void NextWave()
        {
            Encountered++;
            if (Encountered > 1) UpdateHudData();
        }

        public void AddEnemy() => 
            _currentEnemies++;

        public void RemoveEnemy()
        {
            _currentEnemies--;
            EnemyRemoved?.Invoke();
        }

        public int GetEnemies() => _currentEnemies;

        private void UpdateHudData()
        {
            var hudConnectors = GameObject.FindWithTag(WaveChangerTag).GetComponent<PlayersWatcher>().GetConnectors();
            foreach (var hudConnector in hudConnectors) 
                hudConnector.WaveNumber = Encountered;
        }
    }
}