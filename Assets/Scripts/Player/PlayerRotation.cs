using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerRotation : MonoBehaviour
    {
        private const string PointerTag = "Pointer";
        private static Canvas Pointer => 
            GameObject.FindWithTag(PointerTag).GetComponent<Canvas>();

        [SerializeField] private float _speed;
        private int _groundMask;
        private const string GroundMaskName = "Ground";
        private const float CamRayLength = Mathf.Infinity;
        private Vector3 _playerToMouse;
        private const float PointerPositionOffset = 0.15f;

        private void Start()
        {
            Cursor.visible = false;
            _groundMask = LayerMask.GetMask(GroundMaskName);
        }

        private void Update()
        {
            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(camRay, out var groundHit, CamRayLength, _groundMask)) return;
            _playerToMouse = groundHit.point - transform.position;
            _playerToMouse.y = 0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_playerToMouse), _speed * Time.deltaTime);
        }

        private void LateUpdate() => 
            Pointer.transform.position = new Vector3(_playerToMouse.x, PointerPositionOffset, _playerToMouse.z);

        public void SetSpeed(float speed) => 
            _speed = speed;
    }
}