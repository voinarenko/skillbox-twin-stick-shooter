using Cinemachine;
using Mirror;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerCameraConnector : NetworkBehaviour
    {
        private const string VirtualCameraTag = "VirtualCamera";

        [ClientRpc]
        public void Connect(GameObject player)
        {
            var virtualCameras = GameObject.FindGameObjectsWithTag(VirtualCameraTag);
                foreach ( var virtualCam in virtualCameras)
            {
                Debug.Log($"Processing camera: {virtualCam}");
                var virtualCamera = virtualCam.GetComponent<CinemachineVirtualCamera>();
                if (virtualCamera.Follow != null) return;
                virtualCamera.Follow = player.transform;
                virtualCamera.LookAt = player.transform;
            }
        }
    }
}