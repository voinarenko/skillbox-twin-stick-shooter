using System;
using Assets.Scripts.Infrastructure.Services.Audio;
using Assets.Scripts.Infrastructure.Services.Parameters;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.UI.Services.Factory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements.Buttons
{
    public abstract class Button : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        protected IGameStateMachine StateMachine;
        protected IAudioService AudioService;
        protected ISaveLoadService SaveLoadService;
        protected ISettingsService SettingsService;
        protected IUiFactory UiFactory;
        private Image Image => GetComponent<Image>();

        [SerializeField] private Sprite _normal;
        [SerializeField] private Sprite _pressed;
        [SerializeField] private Sprite _hover;

        public void Construct(IUiFactory uiFactory) =>
            UiFactory = uiFactory;
        public void Construct(IGameStateMachine stateMachine) => 
            StateMachine = stateMachine;
        public void Construct(IAudioService audioService) => 
            AudioService = audioService;
        public void Construct(ISaveLoadService saveLoadService, IAudioService audioService, ISettingsService settingsService)
        {
            SaveLoadService = saveLoadService;
            AudioService = audioService;
            SettingsService = settingsService;
        }

        public virtual void OnPointerClick(PointerEventData eventData) => 
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Buttons/Click", GetComponent<Transform>().position);

        public void OnPointerDown(PointerEventData eventData) => 
            Image.sprite = _pressed;

        public void OnPointerUp(PointerEventData eventData) => 
            Image.sprite = _normal;

        public void OnPointerEnter(PointerEventData eventData) => 
            Image.sprite = _hover;

        public void OnPointerExit(PointerEventData eventData) => 
            Image.sprite = _normal;
    }
}