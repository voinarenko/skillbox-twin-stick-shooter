using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.UI.Elements;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public class PerkFactory : IPerkFactory
    {
        private readonly IStaticDataService _staticData;
        private readonly IAssets _assets;

        public PerkFactory(IStaticDataService staticData, IAssets assets)
        {
            _staticData = staticData;
            _assets = assets;
        }

        public async Task<PerkTimer> CreatePerkTimer(Loot loot, GameObject player, Transform perkParent)
        {
            var perkData = _staticData.ForPerk(loot.Type);
            var prefab = await _assets.Load<GameObject>(AssetAddress.PerkElement);
            var perk = Object.Instantiate(prefab, perkParent).GetComponent<PerkTimer>();

            perk.Player = player;
            perk.Type = loot.Type;
            perk.Icon = perkData.Icon;
            perk.Duration = perkData.Duration;
            perk.Multiplier = perkData.Multiplier;

            return perk;
        }
    }
}