using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Elements
{
    public class AmmoCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counter;

        public void UpdateCounter(int currentAmmo) => 
            _counter.text = $"{currentAmmo}";
    }
}