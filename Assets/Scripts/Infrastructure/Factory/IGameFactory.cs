using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        void CreateHud();
    }
}