using Assets.Scripts.StaticData;
using Assets.Scripts.StaticData.Windows;
using Assets.Scripts.UI.Services.Windows;

namespace Assets.Scripts.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void Load();
        EnemyStaticData ForEnemy(EnemyTypeId typeId);
        LevelStaticData ForLevel(string sceneKey);
        WindowConfig ForWindow(WindowId endGame);
    }
}