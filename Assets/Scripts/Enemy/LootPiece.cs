using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.Loot;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class LootPiece : MonoBehaviour
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
        private Loot _loot;
        private Transform _perkParent;
        private bool _picked;

        public void Construct(WorldData worldData, ILootService lootService, Transform perkParent)
        {
            _worldData = worldData;
            _lootService = lootService;
            _perkParent = perkParent;
        }

        public void Initialize(Loot loot)
        {
            _loot = loot;
            Material.Change(Sphere, (int)_loot.Type);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag)) 
                Pickup(other.gameObject);
        }

        private void Pickup(GameObject player)
        {
            if (_picked) return;
            _picked = true;

            UpdateWorldData();

            HideSphere();

            PlayPickupFx();
            ShowText();
            
            _lootService.Process(_loot, player, _perkParent);

            Destroy(gameObject, TimeToDestroy);
        }

        private void UpdateWorldData() => 
            _worldData.LootData.Collect(_loot);

        private void HideSphere() => 
            Sphere.transform.DOScale(0,TimeToHide).OnComplete(() => 
                Sphere.gameObject.SetActive(false));

        private void PlayPickupFx() => 
            Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);

        private void ShowText()
        {
            LootText.text = $"{_loot.Type}";
            PickupPopup.SetActive(true);
        }
    }
}