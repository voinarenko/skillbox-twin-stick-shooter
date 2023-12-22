using UnityEngine;
using Zenject;

namespace Assets.Scripts.Enemy
{
    public class EnemyFactory : IEnemyFactory
    {
        private const string SmallMeleeEnemy = "Enemies/SmallMeleeEnemy";
        private const string BigMeleeEnemy = "Enemies/BigMeleeEnemy";
        private const string RangedEnemy = "Enemies/RangedEnemy";
        private readonly DiContainer _diContainer;

        private Object _smallMeleeEnemyPrefab;
        private Object _bigMeleeEnemyPrefab;
        private Object _rangedEnemyPrefab;

        public EnemyFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void Load()
        {
            _smallMeleeEnemyPrefab = Resources.Load(SmallMeleeEnemy);
            _bigMeleeEnemyPrefab = Resources.Load(BigMeleeEnemy);
            _rangedEnemyPrefab = Resources.Load(RangedEnemy);
        }

        public void Create(EnemyType type, Vector3 at)
        {
            switch (type)
            {
                case EnemyType.SmallMelee:
                    _diContainer.InstantiatePrefab(_smallMeleeEnemyPrefab, at, Quaternion.identity, null);
                    break;
                case EnemyType.BigMelee:
                    _diContainer.InstantiatePrefab(_bigMeleeEnemyPrefab, at, Quaternion.identity, null);
                    break;
                case EnemyType.Ranged:
                    _diContainer.InstantiatePrefab(_rangedEnemyPrefab, at, Quaternion.identity, null);
                    break;
            }
        }
    }
}