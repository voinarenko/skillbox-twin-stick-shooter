using UnityEngine;

namespace Assets.Scripts.Bullet
{
    [CreateAssetMenu(fileName = "Bullet Trail Config", menuName = "ScriptableObject/Bullet Trail Config")]
    public class BulletTrailScriptableObject : ScriptableObject
    {
        public AnimationCurve WidthCurve;
        public float Time = 0.5f;
        public float MinVertexDistance = 0.1f;
        public Gradient ColorGradient;
        public Material Material;

        public void SetupTrail(TrailRenderer trailRenderer)
        {
            trailRenderer.widthCurve = WidthCurve;
            trailRenderer.time = Time;
            trailRenderer.minVertexDistance = MinVertexDistance;
            trailRenderer.colorGradient = ColorGradient;
            trailRenderer.sharedMaterial = Material;
        }
    }
}