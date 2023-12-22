using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerShooter : MonoBehaviour
    {
        private PlayerAudio PlayerAudio => GetComponent<PlayerAudio>();
        private PlayerAnimation PlayerAnimation => GetComponent<PlayerAnimation>();
        private PlayerControls _controls;

        private float _shootTime = float.MinValue;
        [SerializeField] private float _shootDelay;

        private float _reloadTime = float.MinValue;
        [SerializeField] private float _reloadDelay;

        private void Start()
        {
            _controls = new PlayerControls();
            _controls.Enable();
        }

        private void Update()
        {
            var shoot = _controls.Player.Shoot.ReadValue<float>();
            var reload = _controls.Player.Reload.ReadValue<float>();

            if (shoot > 0)
            {
                if (Time.time < _shootTime + _shootDelay) return;

                _shootTime = Time.time;
                PlayerAnimation.Shoot();
                PlayerAudio.Shoot();
                //if (_bullet != null)
                //{
                //    Instantiate(_bullet, _shootPoint.transform.position, transform.rotation);
                //}
#pragma warning disable IDE0059
                shoot = 0;
#pragma warning restore IDE0059
            }

            else if (reload > 0)
            {
                if (!(Time.time < _reloadTime + _reloadDelay))
                {
                    _reloadTime = Time.time;
                    PlayerAnimation.Reload(true);
                    PlayerAudio.Reload();
#pragma warning disable IDE0059
                    reload = 0;
#pragma warning restore IDE0059
                }
                else PlayerAnimation.Reload(false);
            }
        }
    }
}