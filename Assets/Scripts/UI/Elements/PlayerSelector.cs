using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI.Elements.Buttons;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class PlayerSelector : MonoBehaviour
    {
        public LeftButton LeftButton;
        public RightButton RightButton;

        public TextMeshProUGUI PlayerType;
        public Image PlayerImage;

        private IStaticDataService _staticData;
        private readonly Array _playerTypes = Enum.GetValues(typeof(PlayerTypeId));
        private int _playerType;

        public void Construct(IStaticDataService staticData)
        {
            _staticData = staticData;
            UpdateData(0);
        }

        private void Awake()
        {
            LeftButton.Clicked += SwitchLeft;
            RightButton.Clicked += SwitchRight;
        }

        private void OnDestroy()
        {
            LeftButton.Clicked -= SwitchLeft;
            RightButton.Clicked -= SwitchRight;
        }

        private void SwitchLeft()
        {
            _playerType--;
            if (_playerType == -1) _playerType = _playerTypes.Length - 1;
            UpdateData(_playerType);
        }

        private void SwitchRight()
        {
            _playerType++;
            if (_playerType == _playerTypes.Length) _playerType = 0;
            UpdateData(_playerType);
        }

        private void UpdateData(int playerTypeId)
        {
            PlayerType.text = _staticData.ForPlayer((PlayerTypeId)playerTypeId).PlayerTypeId.ToString();
            PlayerImage.sprite = _staticData.ForPlayer((PlayerTypeId)playerTypeId).Image;
        }
    }
}