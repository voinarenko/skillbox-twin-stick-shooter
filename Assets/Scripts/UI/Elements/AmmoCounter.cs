using Assets.Scripts.Data;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Elements
{
    public class AmmoCounter : MonoBehaviour
    {
        public TextMeshProUGUI Counter;
        private WorldData _worldData;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
            _worldData.AmmoData.Changed += UpdateCounter;
            UpdateCounter();
        }

        private void UpdateCounter() => 
            Counter.text = $"{_worldData.AmmoData.Available}";
    }
}