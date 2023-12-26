using System;
using System.Collections.Generic;
using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;

        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<ISavedProgress> ProgressWriters { get; } = new();
        
        public GameObject PlayerGameObject { get; private set; }

        public event Action PlayerCreated;

        public GameFactory(IAssets assets)
        {
            _assets = assets;
        }
        public GameObject CreatePlayer(GameObject at)
        {
            PlayerGameObject = InstantiateRegistered(AssetPath.PlayerPath, at.transform.position);
            PlayerCreated?.Invoke();
            return PlayerGameObject;
        }


        public GameObject CreateHud() => 
            InstantiateRegistered(AssetPath.HudPath);

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            var gameObject = _assets.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            var gameObject = _assets.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter) 
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }
    }
}