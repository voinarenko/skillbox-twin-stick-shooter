using UnityEngine;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "PerkData", menuName = "Static Data/Perk")]
    public class PerkStaticData : ScriptableObject
    {
        public LootTypeId LootTypeId;
        public Sprite Icon;
        public float Duration;
        public float Multiplier;
    }
}