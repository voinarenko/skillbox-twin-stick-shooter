using Mirror;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerLooter : NetworkBehaviour
    {
        [SerializeField] private PlayerShooter _shooter;

        [ClientRpc]
        public void RpcUpdateCollected(int id)
        {
            if (!isLocalPlayer) return;
            _shooter.Storage.UpdateCollected(id);
        }
    }
}