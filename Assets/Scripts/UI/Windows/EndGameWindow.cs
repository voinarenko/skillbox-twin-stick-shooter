using TMPro;

namespace Assets.Scripts.UI.Windows
{
    public class EndGameWindow : BaseWindow
    {
        public TextMeshProUGUI PointsText;
        protected override void Initialize() =>
            RefreshPointsText();

        private void RefreshPointsText() => 
            PointsText.text = $"{Progress.WorldData.LootData.ItemsCollected}";
    }
}