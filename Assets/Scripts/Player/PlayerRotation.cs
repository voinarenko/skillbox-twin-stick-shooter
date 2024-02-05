using Mirror;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerRotation : NetworkBehaviour
    {
        private const string GroundMaskName = "Ground";
        private const float CamRayLength = Mathf.Infinity;
        private const float PointerPositionOffset = 0.15f;

        private static Canvas _pointer;
        [SerializeField] private GameObject _pointerPrefab;

        private float _speed;
        private int _groundMask;

        private void Start()
        {
            _pointer = Instantiate(_pointerPrefab).GetComponent<Canvas>();
            Cursor.visible = false;
            _groundMask = LayerMask.GetMask(GroundMaskName);
        }

        private void Update()
        {
            if (!isOwned) return;
            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(camRay, out var groundHit, CamRayLength, _groundMask)) return;
            var playerToMouse = groundHit.point - transform.position;
            playerToMouse.y = 0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(playerToMouse), _speed * Time.deltaTime);
            _pointer.transform.position = new Vector3(groundHit.point.x, PointerPositionOffset, groundHit.point.z);
        }

        public void SetSpeed(float speed) => 
            _speed = speed;
    }
}