using Mirror;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerScore : NetworkBehaviour
    {
        private DataStorage _storage;
        private const string StorageTag = "Storage";

        private void Start() => 
            _storage = GameObject.FindWithTag(StorageTag).GetComponent<DataStorage>();

        [ClientRpc]
        public void UpdateScore(int score)
        {
            if (isLocalPlayer)
                _storage.PlayerDynamicData.ScoreData.UpdateScore(score);
        }

        [ClientRpc]
        public void RpcUpdateGlobalData(int waves, int smallMelee, int bigMelee, int ranged)
        {
            if (isLocalPlayer)
                _storage.UpdateGlobalData(waves, smallMelee, bigMelee, ranged);
        }
    }
}