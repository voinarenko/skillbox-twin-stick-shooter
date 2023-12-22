using Assets.Scripts.Infrastructure.AssetManagement;

namespace Assets.Scripts.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        public GameFactory(IAssets assets)
        {
            _assets = assets;
        }
        public void CreateHud()
        {
            _assets.Instantiate(AssetPath.HudPath);
        }
    }
}