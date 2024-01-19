using Assets.Scripts.Enemy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class KillData
    {
        public List<KeyValuePair<EnemyType, int>> Killed = new();

        public KillData()
        {
            Killed.Add(new KeyValuePair<EnemyType, int>(EnemyType.SmallMelee, 0));
            Killed.Add(new KeyValuePair<EnemyType, int>(EnemyType.BigMelee, 0));
            Killed.Add(new KeyValuePair<EnemyType, int>(EnemyType.Ranged, 0));
        }

        public void Collect(EnemyAttack enemy)
        {
            foreach (var pair in Killed.ToList().Where(pair => pair.Key == enemy.Type))
            {
                Killed.Add(new KeyValuePair<EnemyType, int>(pair.Key, pair.Value + 1));
                Killed.Remove(pair);
            }
        }
    }
}