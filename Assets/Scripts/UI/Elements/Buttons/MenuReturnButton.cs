using System;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Elements.Buttons
{
    public class MenuReturnButton : Button
    {
        public event Action Clicked;

        public override void OnPointerClick(PointerEventData eventData) => 
            Clicked?.Invoke();
    }
}