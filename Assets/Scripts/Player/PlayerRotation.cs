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

        [SerializeField] private float _speed;
        [SyncVar(hook = nameof(SyncSpeed))]
        private float _syncSpeed;
        private int _groundMask;

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

        public override void OnStartLocalPlayer()
        {
            _pointer = Instantiate(_pointerPrefab).GetComponent<Canvas>();
            Cursor.visible = false;
            _groundMask = LayerMask.GetMask(GroundMaskName);
        }

        [ClientRpc]
        public void RpcSetSpeed(float speed) => 
            _speed = speed;

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

        private void SyncSpeed(float _, float newValue)
        {
            Debug.Log($"Sync in: {newValue}");
            _speed = newValue;
            Debug.Log($"Sync out: {_speed}");
        }
    }
}