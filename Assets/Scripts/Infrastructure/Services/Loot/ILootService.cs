using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.Loot
{
    public interface ILootService : IService
    {
        void Process(Consumable loot, GameObject player);
    }
}