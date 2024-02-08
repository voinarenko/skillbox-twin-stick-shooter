using Mirror;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimator : NetworkBehaviour
    {
        public readonly int AnimIdVertical = Animator.StringToHash("Y");
        public readonly int AnimIdHorizontal = Animator.StringToHash("X");
        public readonly int AnimSpeed = Animator.StringToHash("Speed");

        [SerializeField] private Animator _animator;
        private static readonly int HitHash = Animator.StringToHash("Hit");
        private static readonly int DieHash = Animator.StringToHash("Die");
        private readonly int _animIdAiming = Animator.StringToHash("Aiming");
        private readonly int _animIdShoot = Animator.StringToHash("Shoot");
        private readonly int _animIdReloading = Animator.StringToHash("Reloading");

        [SerializeField] private float _speed;
        [SyncVar(hook = nameof(SyncSpeed))]
        private float _syncSpeed;

        private void Start() => 
            _animator.SetBool(_animIdAiming, true);

        public void Move(Vector3 dir)
        {
            _animator.SetFloat(AnimIdVertical, Vector2.Dot(new Vector2(dir.x, dir.z), new Vector2(transform.forward.x, transform.forward.z)));
            _animator.SetFloat(AnimIdHorizontal, Vector2.Dot(new Vector2(dir.x, dir.z), new Vector2(transform.right.x, transform.right.z)));
        }

        public void Shoot() => 
            _animator.SetTrigger(_animIdShoot);

        public void Reload(bool value) =>
            _animator.SetBool(_animIdReloading, value);

        public void PlayHit() => 
            _animator.SetTrigger(HitHash);

        public void PlayDeath() => 
            _animator.SetTrigger(DieHash);

        [ClientRpc]
        public void RpcSetSpeed(float speed)
        {
            //Debug.Log(speed);
            _speed = speed;
            _animator.SetFloat(AnimSpeed, _speed);
        }

        [Command]
        public void CmdChangeSpeed(float newValue)
        {
            Debug.Log($"Client: {newValue}");
            ChangeSpeedValue(newValue);
        }

        [Server]
        public void ChangeSpeedValue(float newValue)
        {
            Debug.Log($"Server: {newValue}");
            _syncSpeed = newValue;
        }

        private void SyncSpeed(float oldValue, float newValue)
        {
            Debug.Log($"Sync in: {newValue}");
            _speed = newValue;
            _animator.SetFloat(AnimSpeed, _speed);
            Debug.Log($"Sync out: {_speed}");
        }
    }
}