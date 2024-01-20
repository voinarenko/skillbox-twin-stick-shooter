using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.Audio;
using Assets.Scripts.Infrastructure.Services.Parameters;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.States;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        private IPersistentProgressService _progressService;
        protected ISaveLoadService SaveLoadService;
        protected IAudioService AudioService;
        protected ISettingsService SettingsService;
        protected IGameStateMachine StateMachine;
        protected PlayerProgress Progress => _progressService.Progress;

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

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() => 
            Cleanup();

        public virtual void Init(){Debug.Log("Base window init");}
        protected virtual void Initialize(){}
        protected virtual void SubscribeUpdates(){}
        protected virtual void Cleanup(){}
    }
}