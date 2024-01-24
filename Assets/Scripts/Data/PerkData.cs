using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.StaticData;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PerkData
    {
        public List<KeyValuePair<PerkTypeId, int>> Collected = new();

        public PerkData()
        {
            Collected.Add(new KeyValuePair<PerkTypeId, int>(PerkTypeId.Defense, 0));
            Collected.Add(new KeyValuePair<PerkTypeId, int>(PerkTypeId.MoveSpeed, 0));
            Collected.Add(new KeyValuePair<PerkTypeId, int>(PerkTypeId.Damage, 0));
            Collected.Add(new KeyValuePair<PerkTypeId, int>(PerkTypeId.AttackSpeed, 0));
        }

        public void Collect(Perk loot)
        {
            foreach (var pair in Collected.ToList().Where(pair => pair.Key == loot.Type))
            {
                Collected.Add(new KeyValuePair<PerkTypeId, int>(pair.Key, pair.Value + 1));
                Collected.Remove(pair);
            }
        }
    }
}