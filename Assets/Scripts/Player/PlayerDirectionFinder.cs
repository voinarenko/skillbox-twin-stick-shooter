using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerDirectionFinder : MonoBehaviour
    {
        private PlayerAnimation PlayerAnimation => GetComponent<PlayerAnimation>();
        private float _horizontal;
        private float _vertical;

        public int GetDirection()
        {
            var result = 0;
            _horizontal = PlayerAnimation.AnimIdHorizontal;
            _vertical = PlayerAnimation.AnimIdVertical;

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