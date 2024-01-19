using Assets.Scripts.Data;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Elements
{
    public class LootCounter : MonoBehaviour
    {
        public TextMeshProUGUI Counter;
        private WorldData _worldData;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
            UpdateCounter();
        }

        private void UpdateCounter() => 
            Counter.text = $"{_worldData.LootData.ItemsCollected}";
    }
}