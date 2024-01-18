using Assets.Scripts.Data;
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
        private LootMaterial Material => GetComponent<LootMaterial>();
        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;

        public void Construct(WorldData worldData) => 
            _worldData = worldData;

        public void Initialize(Loot loot)
        {
            _loot = loot;
            Material.Change(Sphere, (int)_loot.Type);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag)) 
                Pickup();
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