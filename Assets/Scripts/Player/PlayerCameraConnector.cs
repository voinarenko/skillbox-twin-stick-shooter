using Cinemachine;
using Mirror;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerCameraConnector : NetworkBehaviour
    {
        private const string VirtualCameraTag = "VirtualCamera";

        [ClientRpc]
        public void RpcConnect()
        {
            var virtualCameras = GameObject.FindGameObjectsWithTag(VirtualCameraTag);
                foreach ( var virtualCam in virtualCameras)
            {
                var virtualCamera = virtualCam.GetComponent<CinemachineVirtualCamera>();
                if (virtualCamera.Follow != null) return;
                virtualCamera.Follow = transform;
                virtualCamera.LookAt = transform;
            }
        }
    }
}