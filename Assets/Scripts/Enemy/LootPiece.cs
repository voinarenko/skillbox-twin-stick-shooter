using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.Loot;
using Assets.Scripts.Player;
using Assets.Scripts.StaticData;
using DG.Tweening;
using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class LootPiece : NetworkBehaviour
    {
        private const string PlayerTag = "Player";
        private const float TimeToDestroy = 1.5f;
        private const float TimeToHide = 0.5f;
        [SerializeField] private Renderer _sphere;
        [SerializeField] private GameObject _pickupFxPrefab;
        [SerializeField] private TextMeshPro _lootText;
        [SerializeField] private GameObject _pickupPopup;
        [SerializeField] private Material[] _materials;
        private ILootService _lootService;
        private Consumable _consumable;
        private Perk _perk;
        [SyncVar]
        private int _currentConsumableMaterialId;
        [SyncVar]
        private int _currentPerkMaterialId;
        private bool _picked;
        private bool _isConsumable;

        public void Construct(ILootService lootService) => 
            _lootService = lootService;

        public void Initialize(Consumable loot)
        {
            _consumable = loot;
            _sphere.material = _materials[(int)_consumable.Type];
            _currentConsumableMaterialId = (int)_consumable.Type;
            _isConsumable = true;
        }

        public void Initialize(Perk loot)
        {
            _perk = loot;
            _sphere.material = _materials[(int)_perk.Type];
            _currentPerkMaterialId = (int)_perk.Type;
            _isConsumable = false;
        }

        [ClientRpc]
        public void RpcRefreshMaterial()
        {
            _sphere.material = _isConsumable 
                ? _materials[_currentConsumableMaterialId] 
                : _materials[_currentPerkMaterialId];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isServer) return;
            if (other.CompareTag(PlayerTag)) 
                Pickup(other.GetComponentInParent<PlayerMovement>().gameObject);
        }

        private void Pickup(GameObject player)
        {
            if (_picked) return;
            _picked = true;
            print($"pickup: |{player}|");
            UpdatePlayerData(player);

            RpcHideSphere();

            PlayPickupFx();
            RpcShowText();
            
            if (_isConsumable) _lootService.Process(_consumable, player);
            else player.GetComponent<PlayerHudConnector>().RpcGetPerk(_currentPerkMaterialId);//_lootService.Process(Perk, player, player.GetComponent<PlayerHudConnector>().PerkParent);

            StartCoroutine(DestroyTimer());
        }

        private void UpdatePlayerData(GameObject player) => 
            player.GetComponent<PlayerLooter>().UpdateCollected(_isConsumable ? _currentConsumableMaterialId : _currentPerkMaterialId);

        [ClientRpc]
        private void RpcHideSphere() => 
            _sphere.transform.DOScale(0,TimeToHide).OnComplete(() => 
                _sphere.gameObject.SetActive(false));

        [Server]
        private void PlayPickupFx()
        {
            var effect = Instantiate(_pickupFxPrefab, transform.position, Quaternion.identity);
            NetworkServer.Spawn(effect);
        }

        [ClientRpc]
        private void RpcShowText()
        {
            _lootText.text = _isConsumable ? $"{(ConsumableTypeId)_currentConsumableMaterialId}" : $"{(PerkTypeId)_currentPerkMaterialId}";
            _pickupPopup.SetActive(true);
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(TimeToDestroy);
            DestroySelf();
        }

        [Server]
        private void DestroySelf() => 
            NetworkServer.Destroy(gameObject);
    }
}