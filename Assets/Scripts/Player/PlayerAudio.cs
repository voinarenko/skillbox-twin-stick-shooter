using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        private PlayerDirectionFinder PlayerDirectionFinder => GetComponent<PlayerDirectionFinder>();

        public void FootStep(int index)
        {
            if (index == PlayerDirectionFinder.GetDirection()) 
                FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Move/PlayerFootStep", GetComponent<Transform>().position);
        }

        public void Shoot() => 
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Attack/PlayerShoot", GetComponent<Transform>().position);

        public void Reload() => 
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Attack/PlayerReload", GetComponent<Transform>().position);
    }
}