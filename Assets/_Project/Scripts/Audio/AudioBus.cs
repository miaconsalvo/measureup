using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.Audio
{
    [Serializable]
    public class AudioBus
    {
        public const string PATH_PREFIX = "bus:/";

        public string name;
        public AudioBusType type;
        public string key;
        public string path;
        public float defaultVolume = 0.5f;

        public string Path { get { return PATH_PREFIX + path; } }
    }

    public class RuntimeAudioBus
    {
        public string key;
        public Bus bus;
        public bool IsValid { get { return bus.isValid(); } }

        public RuntimeAudioBus(AudioBus audioBus)
        {
            key = audioBus.key;
            bus = RuntimeManager.GetBus(audioBus.Path);

            if (!bus.isValid()) return;

            if (PlayerPrefs.HasKey(audioBus.key))
                bus.setVolume(PlayerPrefs.GetFloat(audioBus.key));
            else
            {
                bus.setVolume(audioBus.defaultVolume);
                PlayerPrefs.SetFloat(audioBus.key, audioBus.defaultVolume);
            }
        }

        public float Volume
        {
            get { 
                float v = 0f; 
                bus.getVolume(out v);
                return v; }
            set { 
                bus.setVolume(value);
                PlayerPrefs.SetFloat(key, value);
            }
        }
    }

    public enum AudioBusType
    {
        Master,
        Music,
        Sfx,
        UI
    }
}
