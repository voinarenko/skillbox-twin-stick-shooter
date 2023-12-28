using System;
using Assets.Scripts.UI.Services.Windows;
using Assets.Scripts.UI.Windows;

namespace Assets.Scripts.StaticData.Windows
{
    [Serializable]
    public class WindowConfig
    {
        public WindowId WindowId;
        public BaseWindow Prefab;
    }
}