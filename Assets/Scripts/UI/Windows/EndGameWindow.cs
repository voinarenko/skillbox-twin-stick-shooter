using TMPro;

namespace Assets.Scripts.UI.Windows
{
    public class EndGameWindow : BaseWindow
    {
        public TextMeshProUGUI PointsText;

        protected override void Initialize() =>
            RefreshPointsText();

        protected override void SubscribeUpdates() =>
            Progress.WorldData.LootData.Changed += RefreshPointsText;

        protected override void Cleanup()
        {
            base.Cleanup();
            Progress.WorldData.LootData.Changed -= RefreshPointsText;
        }

        private void RefreshPointsText()
        {
            PointsText.text = $"{Progress.WorldData.LootData.Collected}";
        }
    }
}