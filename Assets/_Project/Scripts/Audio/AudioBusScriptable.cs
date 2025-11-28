using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.Audio
{
    [CreateAssetMenu(fileName = "Audio Bus", menuName = "Data/Audio Bus")]
    public class AudioBusScriptable : ScriptableObject
    {
        public string key;
        public string busName;
        public float volume = 1f;
        public Slider slider;

        public void LoadBus()
        {
            //bus = RuntimeManager.GetBus("bus:/" + busName);
            if (PlayerPrefs.HasKey(key))
                volume = PlayerPrefs.GetFloat(key);

            if (slider != null) slider.normalizedValue = volume;
            //bus.setVolume(volume);
        }
    }
}
