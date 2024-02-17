using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Elements
{
    public class AmmoCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counter;

        //public void Construct(PlayerShooter shooter)
        //{
        //    shooter.AmmoChanged.AddListener(UpdateCounter);
        //    UpdateCounter(shooter.PlayerDynamicData.AmmoData.Available);
        //}

        public void UpdateCounter(int currentAmmo)
        {
            print($"Current ammo: |{currentAmmo}|");
            _counter.text = $"{currentAmmo}";
        }
    }
}