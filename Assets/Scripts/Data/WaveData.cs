using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class WaveData
    {
        public int Encountered;
        public Action WaveChanged;
        public Action EnemyAdded;
        public Action EnemyRemoved;

        private int _currentEnemies;

        public void NextWave()
        {
            Encountered++;
            WaveChanged?.Invoke();
        }
 
        public void AddEnemy()
        {
            _currentEnemies++;
            EnemyAdded?.Invoke();
        }

        public void RemoveEnemy()
        {
            _currentEnemies--;
            EnemyRemoved?.Invoke();
        }

        public int GetEnemies() => _currentEnemies;
    }
}