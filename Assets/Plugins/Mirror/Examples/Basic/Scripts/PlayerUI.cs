using UnityEngine;
using UnityEngine.UI;

namespace Assets.Plugins.Mirror.Examples.Basic.Scripts
{
    public class PlayerUi : MonoBehaviour
    {
        [Header("Player Components")]
        public Image Image;

        [Header("Child Text Objects")]
        public Text PlayerNameText;
        public Text PlayerDataText;

        // Sets a highlight color for the local player
        public void SetLocalPlayer()
        {
            // add a visual background for the local player in the UI
            Image.color = new Color(1f, 1f, 1f, 0.1f);
        }

        // This value can change as clients leave and join
        public void OnPlayerNumberChanged(byte newPlayerNumber) => 
            PlayerNameText.text = $"Player {newPlayerNumber:00}";

        // Random color set by Player::OnStartServer
        public void OnPlayerColorChanged(Color newPlayerColor) => 
            PlayerNameText.color = newPlayerColor;

        // This updates from Player::UpdateData via InvokeRepeating on server
        public void OnPlayerDataChanged(ushort newPlayerData)
        {
            // Show the data in the UI
            PlayerDataText.text = $"Data: {newPlayerData:000}";
        }
    }
}
