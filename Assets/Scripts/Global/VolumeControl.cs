using UnityEngine;
using UnityEngine.Audio;

namespace Unexpected
{
    public class VolumeControl : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        public Channel channel = Channel.Master;

        public enum Channel
        {
            Master,
            BGM,
            SFX
        };
        
        public void SetLevel(float sliderValue)
        {
            mixer.SetFloat(channel.ToString(), Mathf.Log10(sliderValue) * 20);
        }
    }
}