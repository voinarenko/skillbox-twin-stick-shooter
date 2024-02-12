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
            ScoreEarnedText.text = $"{Progress.PlayerDynamicData.ScoreData.Score}";
            WavesSurvivedText.text = $"{Progress.WorldData.WaveData.Encountered - 1}";
            BulletsSpentText.text = $"{Progress.PlayerDynamicData.SpentData.Bullets}";
            ReloadsMadeText.text = $"{Progress.PlayerDynamicData.SpentData.Reloads}";
            SmallMeleesKilledText.text = $"{GetValue(EnemyType.SmallMelee)}";
            BigMeleesKilledText.text = $"{GetValue(EnemyType.BigMelee)}";
            RangedKilledText.text = $"{GetValue(EnemyType.Ranged)}";
            HealthPickedText.text = $"{GetValue(ConsumableTypeId.Health)}";
            DefensePickedText.text = $"{GetValue(PerkTypeId.Defense)}";
            MoveSpeedPickedText.text = $"{GetValue(PerkTypeId.MoveSpeed)}";
            DamagePickedText.text = $"{GetValue(PerkTypeId.Damage)}";
            AttackSpeedPickedText.text = $"{GetValue(PerkTypeId.AttackSpeed)}";
        }

        private int GetValue(EnemyType type)
        {
            var result = 0;
            foreach (var pair in Progress.WorldData.KillData.Killed.Where(x => x.Key == type))
                result = pair.Value;
            return result;
        }

        private int GetValue(ConsumableTypeId type)
        {
            var result = 0;
            foreach (var pair in Progress.WorldData.ConsumableData.Collected.Where(x => x.Key == type))
                result = pair.Value;
            return result;
        }
        private int GetValue(PerkTypeId type)
        {
            var result = 0;
            foreach (var pair in Progress.WorldData.PerkData.Collected.Where(x => x.Key == type))
                result = pair.Value;
            return result;
        }
    }
}