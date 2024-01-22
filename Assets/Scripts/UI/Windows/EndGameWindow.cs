using Assets.Scripts.Data;
using Assets.Scripts.Enemy;
using System.Linq;
using Assets.Scripts.Infrastructure.States;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI.Elements.Buttons;
using TMPro;

namespace Assets.Scripts.UI.Windows
{
    public class EndGameWindow : BaseWindow
    {
        public TextMeshProUGUI ScoreEarnedText;
        public TextMeshProUGUI WavesSurvivedText;
        public TextMeshProUGUI BulletsSpentText;
        public TextMeshProUGUI ReloadsMadeText;
        public TextMeshProUGUI SmallMeleesKilledText;
        public TextMeshProUGUI BigMeleesKilledText;
        public TextMeshProUGUI RangedKilledText;
        public TextMeshProUGUI HealthPickedText;
        public TextMeshProUGUI DefensePickedText;
        public TextMeshProUGUI MoveSpeedPickedText;
        public TextMeshProUGUI DamagePickedText;
        public TextMeshProUGUI AttackSpeedPickedText;

        public MenuReturnButton ReturnButton;

        protected override void Initialize()
        {
            ReturnButton.Clicked += ProcessClick;
            DisplayStatistics();
        }

        private void ProcessClick()
        {
            ReturnButton.Clicked -= ProcessClick;
            StateMachine.Enter<BootstrapState>();
        }

        private void DisplayStatistics()
        {
            ScoreEarnedText.text = $"{Progress.WorldData.ScoreData.Score}";
            WavesSurvivedText.text = $"{Progress.WorldData.WaveData.Encountered - 1}";
            BulletsSpentText.text = $"{Progress.WorldData.SpentData.Bullets}";
            ReloadsMadeText.text = $"{Progress.WorldData.SpentData.Reloads}";
            SmallMeleesKilledText.text = $"{GetValue(EnemyType.SmallMelee)}";
            BigMeleesKilledText.text = $"{GetValue(EnemyType.BigMelee)}";
            RangedKilledText.text = $"{GetValue(EnemyType.Ranged)}";
            HealthPickedText.text = $"{GetValue(LootTypeId.Health)}";
            DefensePickedText.text = $"{GetValue(LootTypeId.Defense)}";
            MoveSpeedPickedText.text = $"{GetValue(LootTypeId.MoveSpeed)}";
            DamagePickedText.text = $"{GetValue(LootTypeId.Damage)}";
            AttackSpeedPickedText.text = $"{GetValue(LootTypeId.AttackSpeed)}";
        }

        private int GetValue(EnemyType type)
        {
            var result = 0;
            foreach (var pair in Progress.WorldData.KillData.Killed.Where(x => x.Key == type))
                result = pair.Value;
            return result;
        }

        private int GetValue(LootTypeId type)
        {
            var result = 0;
            foreach (var pair in Progress.WorldData.LootData.Collected.Where(x => x.Key == type))
                result = pair.Value;
            return result;
        }
    }
}