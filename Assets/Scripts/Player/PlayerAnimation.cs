using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator Animator => GetComponent<Animator>();

        public int AnimIdVertical;
        public int AnimIdHorizontal;
        private int _animIdAiming;
        private int _animIdShoot;
        private int _animIdReloading;

        private void Start()
        {
            _animIdAiming = Animator.StringToHash("Aiming");
            AnimIdVertical = Animator.StringToHash("Y");
            AnimIdHorizontal = Animator.StringToHash("X");
            _animIdShoot = Animator.StringToHash("Shoot");
            _animIdReloading = Animator.StringToHash("Reloading");

            Animator.SetBool(_animIdAiming, true);
        }

        public void Move(Vector3 dir)
        {
            Animator.SetFloat(AnimIdVertical, Vector2.Dot(new Vector2(dir.x, dir.z), new Vector2(transform.forward.x, transform.forward.z)));
            Animator.SetFloat(AnimIdHorizontal, Vector2.Dot(new Vector2(dir.x, dir.z), new Vector2(transform.right.x, transform.right.z)));
        }

        public void Shoot() => 
            Animator.SetTrigger(_animIdShoot);

        public void Reload(bool value) =>
            Animator.SetBool(_animIdReloading, value);
    }
}