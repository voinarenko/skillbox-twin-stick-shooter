using Mirror;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerDirectionFinder : NetworkBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerAnimator _playerAnimator;
        private float _horizontal;
        private float _vertical;

        public int GetDirection()
        {
            var result = 99;
            _horizontal = _animator.GetFloat(_playerAnimator.AnimIdHorizontal);
            _vertical = _animator.GetFloat(_playerAnimator.AnimIdVertical);

            if (_vertical > 0 && _vertical > Mathf.Abs(_horizontal)) result = 0;
            else if (_vertical < 0 && Mathf.Abs(_vertical) > Mathf.Abs(_horizontal)) result = 1;
            else if (_horizontal > 0 && _horizontal > Mathf.Abs(_vertical)) result = 2;
            else if (_horizontal < 0 && Mathf.Abs(_horizontal) > Mathf.Abs(_vertical)) result = 3;

            return result;
        }
    }
}