using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.Inputs
{
    public interface IInputService : IService
    {
        Vector2 Axis { get; }

        bool IsAttackButtonUp();
    }
}