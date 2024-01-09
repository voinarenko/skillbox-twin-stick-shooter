using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class Settings
    {
        public Volume Volume;

        public Settings()
        {
            Volume = new Volume();
        }
    }
}