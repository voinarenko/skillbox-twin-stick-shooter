using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : NetworkBehaviour, ISavedProgress
    {
        public float Speed;
        [SyncVar(hook = nameof(SyncSpeed))]
        private float _syncSpeed;

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

        public void UpdateProgress(PlayerProgress progress) => 
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
            {
                var savedPosition = progress.WorldData.PositionOnLevel.Position;
                if (savedPosition != null) 
                    Warp(savedPosition);
            }
        }

        [ClientRpc]
        public void RpcSetSpeed(float speed)
        {
            //Debug.Log(speed);
            Speed = speed;
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
            Speed = newValue;
            Debug.Log($"Sync out: {Speed}");
        }

        private void Warp(Vector3Data to)
        {
            _agent.enabled = false;
            transform.position = to.AsUnityVector();
            _agent.enabled = true;
        }

        private static string CurrentLevel() => 
            SceneManager.GetActiveScene().name;
    }
}