using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.Inputs
{
    public class MobileInputService : InputService
    {
        public override Vector2 Axis => UnityAxis();
    }
}