using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : NetworkBehaviour, ISavedProgress
    {
        public float Speed;

        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private PlayerAnimator _animator;

        private PlayerControls _controls;

        private void Start()
        {
            _controls = new PlayerControls();
            _controls.Enable();
        }

        private void Update()
        {
            if (!isOwned) return;
            var move = _controls.Player.Move.ReadValue<Vector2>();
            var pos = transform.position;
            var dir = new Vector3(move.x, 0, move.y);
            dir.Normalize();

            pos += new Vector3(move.x * Speed, 0, move.y * Speed);
            if (transform != null) transform.position = pos;

            _animator.Move(dir);
        }

        [ClientRpc]
        public void RpcSetSpeed(float speed) => 
            Speed = speed;
    }
}