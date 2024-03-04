using Assets.Scripts.Enemy;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.Player;
using Assets.Scripts.UI.Elements.Buttons;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Windows
{
    public class EndGameWindow : BaseWindow
    {
        [SerializeField] private TextMeshProUGUI _scoreEarnedText;
        [SerializeField] private TextMeshProUGUI _wavesSurvivedText;
        [SerializeField] private TextMeshProUGUI _bulletsSpentText;
        [SerializeField] private TextMeshProUGUI _reloadsMadeText;
        [SerializeField] private TextMeshProUGUI _smallMeleesKilledText;
        [SerializeField] private TextMeshProUGUI _bigMeleesKilledText;
        [SerializeField] private TextMeshProUGUI _rangedKilledText;
        [SerializeField] private TextMeshProUGUI _healthPickedText;
        [SerializeField] private TextMeshProUGUI _defensePickedText;
        [SerializeField] private TextMeshProUGUI _moveSpeedPickedText;
        [SerializeField] private TextMeshProUGUI _damagePickedText;
        [SerializeField] private TextMeshProUGUI _attackSpeedPickedText;

        [SerializeField] private MenuReturnButton _returnButton;

        private const string StorageTag = "Storage";
        private DataStorage _storage;

        protected override void Initialize()
        {
            _storage = GameObject.FindWithTag(StorageTag).GetComponent<DataStorage>();
            _returnButton.Clicked += ProcessClick;
            DisplayStatistics();
        }

        private void ProcessClick()
        {
            _returnButton.Clicked -= ProcessClick;
            Destroy(_storage.gameObject);
            StateMachine.Enter<BootstrapState>();
        }

        private void DisplayStatistics()
        {
            _scoreEarnedText.text = $"{_storage.PlayerDynamicData.ScoreData.Score}";
            _wavesSurvivedText.text = $"{Progress.WorldData.WaveData.Encountered - 1}";
            _bulletsSpentText.text = $"{_storage.PlayerDynamicData.SpentData.Bullets}";
            _reloadsMadeText.text = $"{_storage.PlayerDynamicData.SpentData.Reloads}";
            _smallMeleesKilledText.text = $"{GetValue(EnemyType.SmallMelee)}";
            _bigMeleesKilledText.text = $"{GetValue(EnemyType.BigMelee)}";
            _rangedKilledText.text = $"{GetValue(EnemyType.Ranged)}";
            _healthPickedText.text = $"{_storage.HealthCollected}";
            _defensePickedText.text = $"{_storage.DefenseCollected}";
            _moveSpeedPickedText.text = $"{_storage.MoveSpeedCollected}";
            _damagePickedText.text = $"{_storage.DamageCollected}";
            _attackSpeedPickedText.text = $"{_storage.AttackSpeedCollected}";
        }

        private int GetValue(EnemyType type)
        {
            var result = 0;
            foreach (var pair in Progress.WorldData.KillData.Killed.Where(x => x.Key == type))
                result = pair.Value;
            return result;
        }

        //private int GetValue(ConsumableTypeId type)
        //{
        //    var result = 0;
        //    foreach (var pair in Progress.WorldData.ConsumableData.Collected.Where(x => x.Key == type))
        //        result = pair.Value;
        //    return result;
        //}
        //private int GetValue(PerkTypeId type)
        //{
        //    var result = 0;
        //    foreach (var pair in Progress.WorldData.PerkData.Collected.Where(x => x.Key == type))
        //        result = pair.Value;
        //    return result;
        //}
    }
}