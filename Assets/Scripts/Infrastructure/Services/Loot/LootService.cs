using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.StaticData;
using Assets.Scripts.Player;
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


        public async void Process(Data.Loot loot, GameObject player, Transform perkParent)
        {
            switch (loot.Type)
            {
                case LootTypeId.Health:
                    player.GetComponent<PlayerHealth>().Heal(_staticData.ForConsumable(loot.Type).Amount);
                    break;
                case LootTypeId.AttackSpeed or LootTypeId.Damage or LootTypeId.Defense or LootTypeId.MoveSpeed:
                    var timer = await _perkFactory.CreatePerkTimer(loot, player, perkParent);
                    ApplyPerk(timer, player);
                    timer.Completed += RemovePerk;
                    break;
            }
        }

        private void ApplyPerk(PerkTimer timer, GameObject player)
        {
            var shooter = player.GetComponent<PlayerShooter>();
            switch (timer.Type)
            {
                case LootTypeId.Damage:
                    shooter.Damage *= timer.Multiplier;
                    break;
                case LootTypeId.Defense:
                    player.GetComponent<PlayerHealth>().Defense *= timer.Multiplier;
                    break;
                case LootTypeId.MoveSpeed:
                    player.GetComponent<PlayerMovement>().Speed *= timer.Multiplier;
                    break;
                case LootTypeId.AttackSpeed:
                    shooter.ShootDelay /= timer.Multiplier;
                    shooter.ReloadDelay /= timer.Multiplier;
                    break;
            }
        }

        private void RemovePerk(PerkTimer timer, GameObject player)
        {
            var shooter = player.GetComponent<PlayerShooter>();
            switch (timer.Type)
            {
                case LootTypeId.Damage:
                    shooter.Damage /= timer.Multiplier;
                    break;
                case LootTypeId.Defense:
                    player.GetComponent<PlayerHealth>().Defense /= timer.Multiplier;
                    break;
                case LootTypeId.MoveSpeed:
                    player.GetComponent<PlayerMovement>().Speed /= timer.Multiplier;
                    break;
                case LootTypeId.AttackSpeed:
                    shooter.ShootDelay *= timer.Multiplier;
                    shooter.ReloadDelay *= timer.Multiplier;
                    break;
            }
            Object.Destroy(timer.gameObject);
        }
    }
}