using Assets.Scripts.Data;
using Assets.Scripts.Player;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Elements
{
    public class AmmoCounter : MonoBehaviour
    {
        public TextMeshProUGUI Counter;
        private PlayerShooter _shooter;
        private PlayerDynamicData _playerDynamicData;

        public void Construct(GameObject player, PlayerDynamicData playerDynamicData)
        {
            _shooter = player.GetComponent<PlayerShooter>();
            _playerDynamicData = playerDynamicData;
            _shooter.AmmoChanged += UpdateCounter;
            UpdateCounter();
        }

        private void UpdateCounter() => 
            Counter.text = $"{_playerDynamicData.AmmoData.Available}";
    }
}