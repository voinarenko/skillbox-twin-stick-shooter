using Assets.Scripts.StaticData;
using Assets.Scripts.StaticData.Windows;
using Assets.Scripts.UI.Services.Windows;

namespace Assets.Scripts.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void Load();
        PlayerStaticData ForPlayer(PlayerTypeId typeId);
        EnemyStaticData ForEnemy(EnemyTypeId typeId);
        LevelStaticData ForLevel(string sceneKey);
        WindowConfig ForWindow(WindowId endGame);
        ConsumableStaticData ForConsumable(LootTypeId typeId);
        PerkStaticData ForPerk(LootTypeId typeId);
    }
}