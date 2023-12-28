using Assets.Scripts.Enemy;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.StaticData;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }

        GameObject CreatePlayer(Vector3 at);
        GameObject CreateHud();
        void CreateSpawner(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId);
        void CleanUp();
        GameObject CreateEnemy(EnemyTypeId typeId, Transform parent);
        LootPiece CreateLoot();
    }
}