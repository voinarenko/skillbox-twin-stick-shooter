using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.StaticData;
using System.Collections.Generic;
using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }

        GameObject CreatePlayer(GameObject at);
        GameObject CreateHud();
        void CleanUp();
        GameObject CreateEnemy(EnemyTypeId typeId, Transform parent);
        void Register(EnemySpawner spawner);
    }
}