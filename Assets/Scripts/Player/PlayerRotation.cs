using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private int _groundMask;
        private const string GroundMaskName = "Ground";
        private const float CamRayLength = Mathf.Infinity;

        private void Start()
        {
            _groundMask = LayerMask.GetMask(GroundMaskName);
        }

        private void Update()
        {
            var camRay = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (!Physics.Raycast(camRay, out var groundHit, CamRayLength, _groundMask)) return;
            var playerToMouse = groundHit.point - transform.position;
            playerToMouse.y = 0f;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(playerToMouse), _speed * Time.deltaTime);
        }
    }
}