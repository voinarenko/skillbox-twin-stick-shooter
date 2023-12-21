using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class NpCsMovement
    {
        private Transform _player;

        [Inject]
        private void Construct(PlayerMovement playerMovement)
        {
            _player = playerMovement.transform;
        }
    }
}