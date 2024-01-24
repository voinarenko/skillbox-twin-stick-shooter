using Assets.Scripts.StaticData;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class PerkTimer : MonoBehaviour
    {
        public GameObject Player;
        public PerkTypeId Type;
        public Sprite Icon;
        public float Duration;
        public float Multiplier;
        public event Action<PerkTimer, GameObject> Completed;

        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _timerText;
        private float _timer;

        private void Start()
        {
            _icon.sprite = Icon;
            _timer = Duration;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            DisplayTime(_timer);
            if (_timer <= 0) 
                Completed?.Invoke(this, Player);
        }

        private void DisplayTime(float time)
        {
            if (time < 0) _ = 0;

            var minutes = Mathf.FloorToInt(time / 60);
            var seconds = Mathf.FloorToInt(time % 60);

            _timerText.text = $"{minutes:0}:{seconds:00}";
        }
    }
}