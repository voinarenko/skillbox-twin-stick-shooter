using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerControls _controls;
        [SerializeField] private float _speed;

        private PlayerAnimation PlayerAnimation => GetComponent<PlayerAnimation>();

        private void Start()
        {
            _controls = new PlayerControls();
            _controls.Enable();
        }

        private void Update()
        {
            var move = _controls.Player.Move.ReadValue<Vector2>();
            var pos = transform.position;
            var dir = new Vector3(move.x, 0, move.y);
            dir.Normalize();

            pos += new Vector3(move.x * _speed, 0, move.y * _speed);
            if (transform != null) transform.position = pos;

            PlayerAnimation.Move(dir);
        }
    }
}