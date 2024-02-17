using UnityEngine;

namespace Assets.Plugins.Mirror.Examples.Basic.Scripts
{
    public class CanvasUi : MonoBehaviour
    {
        [Tooltip("Assign Main Panel so it can be turned on from Player:OnStartClient")]
        public RectTransform MainPanel;

        [Tooltip("Assign Players Panel for instantiating PlayerUI as child")]
        public RectTransform PlayersPanel;

        // static instance that can be referenced from static methods below.
        private static CanvasUi _instance;

        private void Awake() => 
            _instance = this;

        public static void SetActive(bool active) => 
            _instance.MainPanel.gameObject.SetActive(active);

        public static RectTransform GetPlayersPanel() => 
            _instance.PlayersPanel;
    }
}
