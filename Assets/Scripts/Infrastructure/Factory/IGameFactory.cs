using Assets.Scripts.Enemy;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.StaticData;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        Task<GameObject> CreateSpawner(Vector3 at, string spawnerId);
        void CleanUp();
        Task<GameObject> CreateEnemy(EnemyTypeId typeId, Transform parent);
        Task<LootPiece> CreateLoot();
        Task WarmUp();
    }
}