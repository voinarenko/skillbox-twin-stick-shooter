using Assets.Scripts.StaticData;

namespace Assets.Scripts.Infrastructure.Services
{
    public interface IStaticDataService : IService
    {
        void Load();
        EnemyStaticData ForEnemy(EnemyTypeId typeId);
        LevelStaticData ForLevel(string sceneKey);
    }
}