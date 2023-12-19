using UnityEngine;


namespace Assets.Scripts
{
    public class PlayerAudio : MonoBehaviour
    {
        private PlayerDirectionFinder PlayerDirectionFinder => GetComponent<PlayerDirectionFinder>();

        public void FootStep(int index)
        {
            if (index == PlayerDirectionFinder.GetDirection()) FMODUnity.RuntimeManager.PlayOneShot("event:/FootStep", GetComponent<Transform>().position);
        }
    }
}