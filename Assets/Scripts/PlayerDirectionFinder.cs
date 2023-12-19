using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerDirectionFinder : MonoBehaviour
    {
        private Animator Animator => GetComponent<Animator>();
        private PlayerMovement PlayerMovement => GetComponent<PlayerMovement>();
        private float _horizontal;
        private float _vertical;

        public int GetDirection()
        {
            var result = 0;
            _horizontal = Animator.GetFloat(PlayerMovement.AnimIdHorizontal);
            _vertical = Animator.GetFloat(PlayerMovement.AnimIdVertical);

            switch (_vertical)
            {
                case > 0:
                {
                    if (_vertical > Mathf.Abs(_horizontal)) result = 0;
                    break;
                }
                case < 0:
                {
                    if (Mathf.Abs(_vertical) > Mathf.Abs(_horizontal)) result = 1;
                    break;
                }
            }

            switch (_horizontal)
            {
                case > 0:
                {
                    if (_horizontal > Mathf.Abs(_vertical)) result = 2;
                    break;
                }
                case < 0:
                {
                    if (Mathf.Abs(_horizontal) > Mathf.Abs(_vertical)) result = 3;
                    break;
                }
            }

            return result;
        }
    }
}