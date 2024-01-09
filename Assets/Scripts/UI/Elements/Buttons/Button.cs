using Assets.Scripts.Infrastructure.States;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements.Buttons
{
    public abstract class Button : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        protected IGameStateMachine StateMachine;
        private Image Image => GetComponent<Image>();

        [SerializeField] private Sprite _normal;
        [SerializeField] private Sprite _pressed;
        [SerializeField] private Sprite _hover;

        public void Construct(IGameStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void OnPointerClick(PointerEventData eventData) { }

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