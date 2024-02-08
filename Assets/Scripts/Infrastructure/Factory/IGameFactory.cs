using Assets.Scripts.Enemy;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.StaticData;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }

        Task<GameObject> CreatePlayer(Vector3 at);
        Task<GameObject> RpcCreateHud();
        Task CreateSpawner(Vector3 at, string spawnerId);
        void CleanUp();
        Task<GameObject> CreateEnemy(EnemyTypeId typeId, Transform parent);
        Task<LootPiece> CreateLoot();
        Task WarmUp();
        Task RpcUpdatePlayerData(GameObject player, PlayerStaticData playerData);
    }
}