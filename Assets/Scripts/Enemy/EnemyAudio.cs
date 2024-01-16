using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyAudio : MonoBehaviour
    {
        public void FootStep() => 
            FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerFootStep", GetComponent<Transform>().position);

        public void Shoot() => 
            FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerShoot", GetComponent<Transform>().position);

        public void Reload() => 
            FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerReload", GetComponent<Transform>().position);
    }
}