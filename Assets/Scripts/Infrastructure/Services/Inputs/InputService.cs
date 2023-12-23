using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.Inputs
{
    public abstract class InputService : IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        public abstract Vector2 Axis { get; }

        public bool IsAttackButtonUp() => 
            UnityEngine.Input.GetMouseButtonUp(0);

        protected static Vector2 UnityAxis() => 
            new(UnityEngine.Input.GetAxis(Horizontal), UnityEngine.Input.GetAxis(Vertical));
    }
}