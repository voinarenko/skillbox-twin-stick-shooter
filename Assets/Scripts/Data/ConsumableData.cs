using Assets.Scripts.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class ConsumableData
    {
        public List<KeyValuePair<ConsumableTypeId, int>> Collected = new();

        public ConsumableData() => 
            Collected.Add(new KeyValuePair<ConsumableTypeId, int>(ConsumableTypeId.Health, 0));

        public void Collect(Consumable loot)
        {
            foreach (var pair in Collected.ToList().Where(pair => pair.Key == loot.Type))
            {
                Collected.Add(new KeyValuePair<ConsumableTypeId, int>(pair.Key, pair.Value + 1));
                Collected.Remove(pair);
            }
        }
    }
}