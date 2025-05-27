using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Mystie
{
    public class AudioManager : MonoBehaviour
    {
        #region Singleton

        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<AudioManager>();
                return instance;
            }
        }

        protected static AudioManager instance;

        #endregion

        void Awake()
        {
            //If an instance already exists
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        public void PlayOneShot(EventReference sound)
        {
            RuntimeManager.PlayOneShot(sound);
        }

        public void PlayOneShot(EventReference sound, Vector3 worldPos)
        {
            RuntimeManager.PlayOneShot(sound, worldPos);
        }

        public EventInstance CreateInstance(EventReference eventRefrence)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventRefrence);
            return eventInstance;
        }
    }
}
