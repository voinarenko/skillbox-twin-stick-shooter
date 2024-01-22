using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.StaticData;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class LootData
    {
        public List<KeyValuePair<LootTypeId, int>> Collected = new();

        public LootData()
        {
            Collected.Add(new KeyValuePair<LootTypeId, int>(LootTypeId.Health, 0));
            Collected.Add(new KeyValuePair<LootTypeId, int>(LootTypeId.Defense, 0));
            Collected.Add(new KeyValuePair<LootTypeId, int>(LootTypeId.MoveSpeed, 0));
            Collected.Add(new KeyValuePair<LootTypeId, int>(LootTypeId.Damage, 0));
            Collected.Add(new KeyValuePair<LootTypeId, int>(LootTypeId.AttackSpeed, 0));
        }

        public void Collect(Loot loot)
        {
            foreach (var pair in Collected.ToList().Where(pair => pair.Key == loot.Type))
            {
                Collected.Add(new KeyValuePair<LootTypeId, int>(pair.Key, pair.Value + 1));
                Collected.Remove(pair);
            }
        }
    }
}