using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Elements.Buttons
{
    public class QuitButton : Button
    {
        public override void OnPointerClick(PointerEventData eventData) => Application.Quit();
    }
}