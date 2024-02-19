using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Elements
{
    public class WaveCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counter;

        public void UpdateCounter(int currentWave)
        {
            print($"Waves |{currentWave}|");
            _counter.text = $"{currentWave}";
        }
    }
}