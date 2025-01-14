﻿using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.Audio;
using Assets.Scripts.Infrastructure.Services.Parameters;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Infrastructure.States;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        protected ISaveLoadService SaveLoadService;
        protected IAudioService AudioService;
        protected ISettingsService SettingsService;
        protected IGameStateMachine StateMachine;
        protected IStaticDataService StaticData;
        protected PlayerProgress Progress => _progressService.Progress;
        private IPersistentProgressService _progressService;

        public void Construct(IPersistentProgressService progressService, IGameStateMachine stateMachine)
        {
            _progressService = progressService;
            StateMachine = stateMachine;
        }

        public void Construct(IAudioService audioService) => 
            AudioService = audioService;

        public void Construct(ISaveLoadService saveLoadService, IAudioService audioService, ISettingsService settingsService)
        {
            SaveLoadService = saveLoadService;
            AudioService = audioService;
            SettingsService = settingsService;
        }
        public void Construct(IPersistentProgressService progressService, ISaveLoadService saveLoadService, IAudioService audioService, ISettingsService settingsService, IGameStateMachine stateMachine, IStaticDataService staticData)
        {
            _progressService = progressService;
            SaveLoadService = saveLoadService;
            AudioService = audioService;
            SettingsService = settingsService;
            StateMachine = stateMachine;
            StaticData = staticData;
        }

        protected virtual void Start()
        {
            Cursor.visible = true;
            Initialize();
            SubscribeUpdates();
        }

        protected virtual void OnDestroy() => 
            Cleanup();

        public virtual void Init(){Debug.Log("Base window init");}
        protected virtual void Initialize(){}
        protected virtual void SubscribeUpdates(){}
        protected virtual void Cleanup(){}
    }
}