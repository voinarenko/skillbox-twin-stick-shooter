using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.Inputs
{
    public class StandaloneInputService : InputService
    {
        public override Vector2 Axis
        {
            get
            {
                var axis = UnityAxis();

                if (axis == Vector2.zero)
                {
                    axis = UnityAxis();
                }
                return axis;
            }
        }
    }
}