using UnityEngine;

namespace Assets.Scripts.UI.Elements
{
    public class PerkDisplay : MonoBehaviour
    {
        [SerializeField] private Transform _parent;

        public Transform GetParent() => _parent;
    }
}