using System;
using System.Collections.Generic;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject PlayerGameObject { get; }
        event Action PlayerCreated;

        GameObject CreatePlayer(GameObject at);
        GameObject CreateHud();
        void CleanUp();
    }
}