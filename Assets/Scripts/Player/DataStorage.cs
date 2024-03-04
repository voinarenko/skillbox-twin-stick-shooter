using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class DataStorage : MonoBehaviour
    {
        public PlayerDynamicData PlayerDynamicData;// { get; set; }
        public int HealthCollected;// { get; private set; }
        public int DefenseCollected;// { get; private set; }
        public int MoveSpeedCollected;// { get; private set; }
        public int DamageCollected;// { get; private set; }
        public int AttackSpeedCollected;// { get; private set; }

        private void Start()
        {
            PlayerDynamicData = new PlayerDynamicData();
            DontDestroyOnLoad(this);
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