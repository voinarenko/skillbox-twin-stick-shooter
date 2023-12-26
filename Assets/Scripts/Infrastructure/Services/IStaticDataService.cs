using Assets.Scripts.StaticData;

namespace Assets.Scripts.Infrastructure.Services
{
    public interface IStaticDataService : IService
    {
        void LoadEnemies();
        EnemyStaticData ForEnemy(EnemyTypeId typeId);
    }
}