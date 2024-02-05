using Mirror;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimator : NetworkBehaviour
    {
        public readonly int AnimIdVertical = Animator.StringToHash("Y");
        public readonly int AnimIdHorizontal = Animator.StringToHash("X");
        public readonly int AnimSpeed = Animator.StringToHash("Speed");

        private Animator Animator => GetComponent<Animator>();
        private static readonly int HitHash = Animator.StringToHash("Hit");
        private static readonly int DieHash = Animator.StringToHash("Die");
        private readonly int _animIdAiming = Animator.StringToHash("Aiming");
        private readonly int _animIdShoot = Animator.StringToHash("Shoot");
        private readonly int _animIdReloading = Animator.StringToHash("Reloading");

        private void Start() => 
            Animator.SetBool(_animIdAiming, true);

        public void Move(Vector3 dir)
        {
            Animator.SetFloat(AnimIdVertical, Vector2.Dot(new Vector2(dir.x, dir.z), new Vector2(transform.forward.x, transform.forward.z)));
            Animator.SetFloat(AnimIdHorizontal, Vector2.Dot(new Vector2(dir.x, dir.z), new Vector2(transform.right.x, transform.right.z)));
        }

        public void Shoot() => 
            Animator.SetTrigger(_animIdShoot);

        public void Reload(bool value) =>
            Animator.SetBool(_animIdReloading, value);

        public void PlayHit() => 
            Animator.SetTrigger(HitHash);

        public void PlayDeath() => 
            Animator.SetTrigger(DieHash);
    }
}