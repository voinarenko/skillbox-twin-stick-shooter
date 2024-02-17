using Mirror;

namespace Assets.Scripts.Infrastructure
{
    public struct CreatePlayerMessage : NetworkMessage
    {
        public int PlayerType;
        public int Health;
        public int Ammo;
        public float MoveSpeed;
        public float RotateSpeed;
        public float SpeedFactor;
        public float Damage;
        public float AttackCooldown;
        public float ReloadCooldown;
    }
}