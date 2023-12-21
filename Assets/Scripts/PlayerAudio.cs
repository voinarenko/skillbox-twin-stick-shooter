using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerAudio : MonoBehaviour
    {
        private PlayerDirectionFinder PlayerDirectionFinder => GetComponent<PlayerDirectionFinder>();

        public void FootStep(int index)
        {
            if (index == PlayerDirectionFinder.GetDirection()) FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerFootStep", GetComponent<Transform>().position);
        }

        public void Shoot()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerShoot", GetComponent<Transform>().position);
        }

        public void Reload()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerReload", GetComponent<Transform>().position);
        }
    }
}