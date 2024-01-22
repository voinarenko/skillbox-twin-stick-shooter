using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.Loot
{
    public interface ILootService : IService
    {
        void Process(Data.Loot loot, GameObject player);
    }
}