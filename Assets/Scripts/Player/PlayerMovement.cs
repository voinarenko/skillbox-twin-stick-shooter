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
        private NavMeshAgent Agent => GetComponent<NavMeshAgent>();
        [SerializeField] private float _speed;

        private PlayerAnimator PlayerAnimator => GetComponent<PlayerAnimator>();

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

        public void ControlsEnabled(bool value)
        {
            if (value) _controls.Enable();
            else _controls.Disable();
        }

        public void SetSpeed(float speed) => 
            _speed = speed;

        private void Warp(Vector3Data to)
        {
            // if physics is being used — disable, then re-enable
            Agent.enabled = false;
            transform.position = to.AsUnityVector();
            Agent.enabled = true;
        }

        private static string CurrentLevel() => 
            SceneManager.GetActiveScene().name;
    }
}