using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.UI.Elements;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public interface IPerkFactory : IService
    {
        Task<PerkTimer> CreatePerkTimer(Perk perk, Transform perkParent);
    }
}