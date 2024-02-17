using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Elements
{
    public class WaveCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counter;
        //private WorldData _worldData;

        //public void Construct(WorldData worldData)
        //{
        //    _worldData = worldData;
        //    _worldData.WaveData.WaveChanged += UpdateCounter;
        //    UpdateCounter();
        //}

        public void UpdateCounter(int currentWave)
        {
            print($"Waves |{currentWave}|");
            _counter.text = $"{currentWave}";
        }
    }
}