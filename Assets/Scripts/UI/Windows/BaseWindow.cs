using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.Audio;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        private IPersistentProgressService _progressService;
        protected ISaveLoadService SaveLoadService;
        protected IAudioService AudioService;
        //protected IGameStateMachine StateMachine;
        protected PlayerProgress Progress => _progressService.Progress;

        public void Construct(IPersistentProgressService progressService) => 
            _progressService = progressService;

        public void Construct(ISaveLoadService saveLoadService, IAudioService audioService)
        {
            SaveLoadService = saveLoadService;
            AudioService = audioService;
        }
        //public void Construct(IGameStateMachine stateMachine) => 
        //    StateMachine = stateMachine;

        //private void Awake() => OnAwake();

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() => 
            Cleanup();

        //protected virtual void OnAwake() => CloseButton.onClick.AddListener(() => Destroy(gameObject));

        protected virtual void Initialize(){}
        protected virtual void SubscribeUpdates(){}
        protected virtual void Cleanup(){}
    }
}