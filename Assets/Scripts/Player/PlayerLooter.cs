using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerLooter : MonoBehaviour
    {
        [SerializeField] private PlayerShooter _shooter;

        public void UpdateCollected(int id) => 
            _shooter.Storage.UpdateCollected(id);
    }
}