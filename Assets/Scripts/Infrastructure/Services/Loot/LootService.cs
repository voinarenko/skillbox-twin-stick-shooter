using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.Loot
{
    public class LootService : ILootService
    {
        private readonly IStaticDataService _staticData;

        public LootService(IStaticDataService staticData) => 
            _staticData = staticData;

        public void Process(Consumable loot, GameObject player) => 
            player.GetComponent<PlayerHealth>().RpcHeal(_staticData.ForConsumable(loot.Type).Amount);
    }
}