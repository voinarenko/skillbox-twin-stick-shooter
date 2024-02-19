using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Elements
{
    public class WaveCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counter;

        public void UpdateCounter(int currentWave) => 
            _counter.text = $"{currentWave}";
    }
}