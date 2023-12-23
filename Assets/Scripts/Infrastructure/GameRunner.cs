using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        public GameBootstrapper BootstrapperPrefab;
        private void Awake()
        {
            var bootstrapper = FindAnyObjectByType<GameBootstrapper>();

            if (bootstrapper) return;

            Instantiate(BootstrapperPrefab);
        }
    }
}