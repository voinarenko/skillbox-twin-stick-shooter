using System.Collections;
using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.Loot;
using Assets.Scripts.Player;
using DG.Tweening;
using Mirror;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class LootPiece : NetworkBehaviour
    {
        public Renderer Sphere;
        public GameObject PickupFxPrefab;
        public TextMeshPro LootText;
        public GameObject PickupPopup;

        private const string PlayerTag = "Player";
        private const float TimeToDestroy = 1.5f;
        private const float TimeToHide = 0.5f;
        private ILootService _lootService;
        private LootMaterial Material => GetComponent<LootMaterial>();
        private WorldData _worldData;
        private Consumable _consumable;
        private Perk _perk;
        private bool _picked;
        private bool _isConsumable;

        public void Construct(WorldData worldData, ILootService lootService)
        {
            _worldData = worldData;
            _lootService = lootService;
        }

        public void Initialize(Consumable loot)
        {
            _consumable = loot;
            Material.Change(Sphere, (int)_consumable.Type);
            _isConsumable = true;
        }
        public void Initialize(Perk loot)
        {
            _perk = loot;
            Material.Change(Sphere, (int)_perk.Type);
            _isConsumable = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag)) 
                Pickup(other.GetComponentInParent<PlayerMovement>().gameObject);
        }

        private void Pickup(GameObject player)
        {
            if (_picked) return;
            _picked = true;

            UpdateWorldData();

            HideSphere();

            PlayPickupFx();
            ShowText();
            
            if (_isConsumable) _lootService.Process(_consumable, player);
            else _lootService.Process(_perk, player, player.GetComponent<PlayerHudConnector>().PerkParent);

            StartCoroutine(DestroyTimer());
        }

        private void UpdateWorldData()
        {
            if (_isConsumable) _worldData.ConsumableData.Collect(_consumable);
            else _worldData.PerkData.Collect(_perk);
        }

        private void HideSphere() => 
            Sphere.transform.DOScale(0,TimeToHide).OnComplete(() => 
                Sphere.gameObject.SetActive(false));

        private void PlayPickupFx()
        {
            var effect = Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);
            NetworkServer.Spawn(effect);
        }

        private void ShowText()
        {
            LootText.text = _isConsumable ? $"{_consumable.Type}" : $"{_perk.Type}";
            PickupPopup.SetActive(true);
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