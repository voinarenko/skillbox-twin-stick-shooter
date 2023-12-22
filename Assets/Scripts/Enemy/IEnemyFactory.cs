using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public interface IEnemyFactory
    {
        void Load();
        void Create(EnemyType type, Vector3 at);
    }
}