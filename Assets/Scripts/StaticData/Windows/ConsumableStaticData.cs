using UnityEngine;

namespace Assets.Scripts.StaticData.Windows
{
    [CreateAssetMenu(fileName = "ConsumableData", menuName = "Static Data/Consumable")]
    public class ConsumableStaticData : ScriptableObject
    {
        public LootTypeId LootTypeId;
        public int Amount;
    }
}