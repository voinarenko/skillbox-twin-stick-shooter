using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerControls _controls;
        [SerializeField] private float _speed;

        private Animator Animator => GetComponent<Animator>();
        public int AnimIdVertical;
        public int AnimIdHorizontal;
        private int _animIdAiming;

        private void Start()
        {
            _controls = new PlayerControls();
            _controls.Enable();

            _animIdAiming = Animator.StringToHash("Aiming");
            AnimIdVertical = Animator.StringToHash("Y");
            AnimIdHorizontal = Animator.StringToHash("X");

            Animator.SetBool(_animIdAiming, true);
        }

        private void Update()
        {
            var move = _controls.Player.Move.ReadValue<Vector2>();
            var pos = transform.position;
            var dir = new Vector3(move.x, 0, move.y);
            dir.Normalize();

            pos += new Vector3(move.x * _speed, 0, move.y * _speed);
            if (transform != null) transform.position = pos;

            Animator.SetFloat(AnimIdVertical, Vector2.Dot(new Vector2(dir.x, dir.z), new Vector2(transform.forward.x, transform.forward.z)));
            Animator.SetFloat(AnimIdHorizontal, Vector2.Dot(new Vector2(dir.x, dir.z), new Vector2(transform.right.x, transform.right.z)));
        }
    }
}
