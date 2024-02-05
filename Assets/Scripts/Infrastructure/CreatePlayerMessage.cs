using Mirror;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public struct CreatePlayerMessage : NetworkMessage
    {
        public int PlayerType;

    }
}