using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Player;
using Assets.Scripts.Data;
using Assets.Scripts.Logic;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI.Elements;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.Loot
{
    public class LootService : ILootService
    {
        private readonly IPerkFactory _perkFactory;
        private readonly IStaticDataService _staticData;

        public LootService(IPerkFactory perkFactory, IStaticDataService staticData)
        {
            _staticData = staticData;
            _perkFactory = perkFactory;
        }
        
        public void Process(Consumable loot, GameObject player) => 
            player.GetComponent<PlayerHealth>().Heal(_staticData.ForConsumable(loot.Type).Amount);

        public async void Process(Perk loot, GameObject player, Transform perkParent)
        {
            var timer = await _perkFactory.CreatePerkTimer(loot, perkParent);
            ApplyPerk(timer, player);
            timer.Completed += RemovePerk;
        }

        private static void ApplyPerk(PerkTimer timer, GameObject player)
        {
            timer.Player = player;
            var shooter = player.GetComponent<PlayerShooter>();
            switch (timer.Type)
            {
                case PerkTypeId.Damage:
                    shooter.Damage *= timer.Multiplier;
                    break;
                case PerkTypeId.Defense:
                    player.GetComponent<PlayerHealth>().Defense *= timer.Multiplier;
                    break;
                case PerkTypeId.MoveSpeed:
                    player.GetComponent<PlayerMovement>().Speed *= timer.Multiplier;
                    break;
                case PerkTypeId.AttackSpeed:
                    shooter.ShootDelay /= timer.Multiplier;
                    shooter.ReloadDelay /= timer.Multiplier;
                    break;
            }
        }

        private static void RemovePerk(PerkTimer timer, GameObject player)
        {
            var shooter = player.GetComponent<PlayerShooter>();
            switch (timer.Type)
            {
                case PerkTypeId.Damage:
                    shooter.Damage /= timer.Multiplier;
                    break;
                case PerkTypeId.Defense:
                    player.GetComponent<PlayerHealth>().Defense /= timer.Multiplier;
                    break;
                case PerkTypeId.MoveSpeed:
                    player.GetComponent<PlayerMovement>().Speed /= timer.Multiplier;
                    break;
                case PerkTypeId.AttackSpeed:
                    shooter.ShootDelay *= timer.Multiplier;
                    shooter.ReloadDelay *= timer.Multiplier;
                    break;
            }
            Object.Destroy(timer.gameObject);
        }
    }
}