using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class AmmoData
    {
        public int Available;

        //public void Consume(Ammo ammo)
        //{
        //    Available -= ammo.Value;
        //    Changed.Invoke();
        //}
    }
}