using Assets.Scripts.Data;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class LootPiece : MonoBehaviour
    {
        public GameObject Sphere;
        public GameObject PickupFxPrefab;
        public TextMeshPro lootText;
        public GameObject PickupPopup;

        private const float TimeToDestroy = 1.5f;
        private const string PlayerTag = "Player";
        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;

        public void Construct(WorldData worldData) => 
            _worldData = worldData;

        public void Initialize(Loot loot) => _loot = loot;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag)) Pickup();
        }

        private void Pickup()
        {
            if (_picked) return;
            _picked = true;

            UpdateWorldData();

            HideSphere();

            PlayPickupFx();
            ShowText();
            
            Destroy(gameObject, TimeToDestroy);
        }

        private void UpdateWorldData() => 
            _worldData.LootData.Collect(_loot);

        private void HideSphere() => 
            Sphere.SetActive(false);

        private void PlayPickupFx() => 
            Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);

        private void ShowText()
        {
            lootText.text = $"{_loot.Type}";
            PickupPopup.SetActive(true);
        }
    }
}