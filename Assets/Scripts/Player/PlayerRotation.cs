using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerRotation : MonoBehaviour
    {
        private const string GroundMaskName = "Ground";
        private const string PointerTag = "Pointer";
        private const float CamRayLength = Mathf.Infinity;
        private const float PointerPositionOffset = 0.15f;
        private static Canvas _pointer;


        private float _speed;
        private int _groundMask;

        private void Start()
        {
            _pointer = GameObject.FindWithTag(PointerTag).GetComponent<Canvas>();
            Cursor.visible = false;
            _groundMask = LayerMask.GetMask(GroundMaskName);
        }

        private void Update()
        {
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