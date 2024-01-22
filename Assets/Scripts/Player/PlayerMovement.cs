using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour, ISavedProgress
    {
        private PlayerControls _controls;
        private NavMeshAgent _agent;
        public float Speed;

        private PlayerAnimator PlayerAnimator => GetComponent<PlayerAnimator>();

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _controls = new PlayerControls();
            _controls.Enable();
        }

        private void Update()
        {
            var move = _controls.Player.Move.ReadValue<Vector2>();
            var pos = transform.position;
            var dir = new Vector3(move.x, 0, move.y);
            dir.Normalize();

            pos += new Vector3(move.x * Speed, 0, move.y * Speed);
            if (transform != null) transform.position = pos;

            PlayerAnimator.Move(dir);
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

        public void SetSpeed(float speed) => 
            Speed = speed;

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