using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class VcaController : MonoBehaviour
    {
        public string VcaName;
        private FMOD.Studio.VCA _vca;

        private void Start() => 
            _vca = FMODUnity.RuntimeManager.GetVCA($"vca:/{VcaName}");

        public void SetVolume(float volume) => 
            _vca.setVolume(volume);
    }
}