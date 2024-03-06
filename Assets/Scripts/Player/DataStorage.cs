using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class DataStorage : MonoBehaviour
    {
        public PlayerDynamicData PlayerDynamicData { get; private set; }
        public int WavesEncountered { get; private set; }
        public int SmallMeleeEnemyKilled { get; private set; }
        public int BigMeleeEnemyKilled { get; private set; }
        public int RangedEnemyKilled { get; private set; }
        public int HealthCollected { get; private set; }
        public int DefenseCollected { get; private set; }
        public int MoveSpeedCollected { get; private set; }
        public int DamageCollected { get; private set; }
        public int AttackSpeedCollected { get; private set; }

        private void Start()
        {
            PlayerDynamicData = new PlayerDynamicData();
            DontDestroyOnLoad(this);
        }

        public void UpdateGlobalData(int waves, int smallMelee, int bigMelee, int ranged)
        {
            WavesEncountered = waves;
            SmallMeleeEnemyKilled = smallMelee;
            BigMeleeEnemyKilled = bigMelee;
            RangedEnemyKilled = ranged;
        }

        public void UpdateCollected(int id)
        {
            switch (id)
            {
                case 0:
                    HealthCollected++;
                    break;
                case 1:
                    DefenseCollected++;
                    break;
                case 2:
                    MoveSpeedCollected++;
                    break;
                case 3:
                    DamageCollected++;
                    break;
                case 4: 
                    AttackSpeedCollected++;
                    break;
            }
        }
    }
}