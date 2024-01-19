using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class LootData
    {
        public List<KeyValuePair<LootType, int>> Collected = new();
        public int ItemsCollected;

        public LootData()
        {
            Collected.Add(new KeyValuePair<LootType, int>(LootType.Health, 0));
            Collected.Add(new KeyValuePair<LootType, int>(LootType.Defense, 0));
            Collected.Add(new KeyValuePair<LootType, int>(LootType.MoveSpeed, 0));
            Collected.Add(new KeyValuePair<LootType, int>(LootType.Damage, 0));
            Collected.Add(new KeyValuePair<LootType, int>(LootType.AttackSpeed, 0));
        }

        public void Collect(Loot loot)
        {
            foreach (var pair in Collected.ToList().Where(pair => pair.Key == loot.Type))
            {
                Collected.Add(new KeyValuePair<LootType, int>(pair.Key, pair.Value + 1));
                Collected.Remove(pair);
            }
        }
    }
}