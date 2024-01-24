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

        public async Task<PerkTimer> CreatePerkTimer(Perk perk, Transform perkParent)
        {
            var perkData = _staticData.ForPerk(perk.Type);
            var prefab = await _assets.Load<GameObject>(AssetAddress.PerkElement);
            var perkObject = Object.Instantiate(prefab, perkParent).GetComponent<PerkTimer>();

            perkObject.Type = perk.Type;
            perkObject.Icon = perkData.Icon;
            perkObject.Duration = perkData.Duration;
            perkObject.Multiplier = perkData.Multiplier;

            return perkObject;
        }
    }
}