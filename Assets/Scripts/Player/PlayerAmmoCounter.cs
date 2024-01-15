using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAmmoCounter : MonoBehaviour
    {
        public int MaxAmmo;

        private PlayerShooter Shooter => GetComponent<PlayerShooter>();
        
        private const int MinAmmo = 0;
        private int _currentAmmo;


        public bool Check() => 
            _currentAmmo > MinAmmo;

        private void Awake() => 
            Shooter.Shot += OnShot;

        private void OnDestroy() => 
            Shooter.Shot -= OnShot;

        public void Reset() => 
            _currentAmmo = MaxAmmo;

        private void OnShot() => 
            _currentAmmo--;
    }
}