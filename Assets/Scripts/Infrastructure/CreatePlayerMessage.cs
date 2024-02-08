using Mirror;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public struct CreatePlayerMessage : NetworkMessage
    {
        public int PlayerType;
        public int Ammo;
        public float MoveSpeed;
        public float RotateSpeed;
        public float SpeedFactor;
    }
}