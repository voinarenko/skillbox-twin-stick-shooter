using System;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Elements.Buttons
{
    public class LeftButton : Button
    {
        public event Action Clicked;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            Clicked?.Invoke();
        }
    }
}