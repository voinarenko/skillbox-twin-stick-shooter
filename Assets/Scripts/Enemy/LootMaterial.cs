using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class LootMaterial : MonoBehaviour
    {
        [SerializeField] private Material[] _materials;

        public void Change(Renderer sphere, int materialIndex) => 
            sphere.material = _materials[materialIndex];
    }
}