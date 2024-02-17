using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class WaveData
    {
        public int Encountered;
        public event Action<int> Changed;
        public event Action EnemyAdded;
        public event Action EnemyRemoved;

        private int _currentEnemies;

        public void NextWave()
        {
            Encountered++;
            Changed?.Invoke(Encountered);
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