using Assets.Scripts.Player;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Enemy
{
    public class EnemyMovement
    {
        private Transform _player;

        [Inject]
        private void Construct(PlayerMovement playerMovement)
        {
            _player = playerMovement.transform;
        }
    }
}