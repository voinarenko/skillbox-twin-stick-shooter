using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class WaveData
    {
        public int Encountered;
        public Action Changed;

        public void NextWave()
        {
            Encountered++;
            Changed?.Invoke();
        }
    }
}