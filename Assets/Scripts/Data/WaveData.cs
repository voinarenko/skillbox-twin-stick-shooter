using System;
using Assets.Scripts.Player;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class WaveData
    {
        public int Encountered;
        public event Action Changed;
        public event Action EnemyRemoved;

        private int _currentEnemies;

        public void NextWave()
        {
            Encountered++;
            Changed?.Invoke();
            if (Encountered > 1) Object.FindAnyObjectByType<PlayerHudConnector>().WaveNumber = Encountered;
        }

        public void AddEnemy() => 
            _currentEnemies++;

        public void RemoveEnemy()
        {
            _currentEnemies--;
            EnemyRemoved?.Invoke();
        }

        public int GetEnemies() => _currentEnemies;
    }
}