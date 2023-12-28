﻿using Assets.Scripts.UI.Services.Factory;

namespace Assets.Scripts.UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.Unknown:
                    break;
                case WindowId.EndGame:
                    _uiFactory.CreateEnGame();
                    break;
            }
        }
    }
}