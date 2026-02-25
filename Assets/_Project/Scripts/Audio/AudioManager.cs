using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mystie
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private bool playOnStart;
        [SerializeField] private EventInstance bgmEmitter;
        [SerializeField] private EventReference bgm;
        [SerializeField] private float bgmVolume = 1f;
        [SerializeField] private AudioCollection loadAudio;

        private Dictionary<string, EventInstance> audioInstances;
        private Dictionary<string, EventReference> audioDict;

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

            audioInstances = new Dictionary<string, EventInstance>();
            audioDict = loadAudio != null ? loadAudio.audioDict : new Dictionary<string, EventReference>();
        }

        public void Start()
        {
            if (!bgm.IsNull)
                SetMusic(bgm, bgmVolume, playOnStart);
        }

        private void OnDestroy()
        {
            if (bgmEmitter.isValid()) bgmEmitter.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            StopAudioAll();
        }

        #region Music

        public void PlayMusic()
        {
            if (bgmEmitter.isValid())
            {
                SetMusicVolume(bgmVolume);
                bgmEmitter.start();
            }
        }

        public void PlayMusic(float volume)
        {
            if (bgmEmitter.isValid())
            {
                SetMusicVolume(volume);
                bgmEmitter.start();
            }
        }

        public void PauseMusic(bool pause = true)
        {
            if (bgmEmitter.isValid())
                bgmEmitter.setPaused(pause);
        }

        public void StopMusic()
        {
            if (bgmEmitter.isValid())
                bgmEmitter.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        public void SetMusic(EventReference eventReference, float volume = 1, bool play = true, float delay = 0)
        {
            if (bgmEmitter.isValid() && bgm.Equals(eventReference)) return;

            StopMusic();
            if (!eventReference.IsNull)
            {
                bgm = eventReference;
                bgmEmitter = RuntimeManager.CreateInstance(bgm);
                if (play) StartCoroutine(SetMusicCoroutine(volume, delay));
            }
        }

        public IEnumerator SetMusicCoroutine(float volume = 1, float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            PlayMusic(volume);
        }

        public void SetMusicVolume(float volume = 1f)
        {
            if (bgmEmitter.isValid())
            {
                bgmVolume = volume;
                bgmEmitter.setVolume(bgmVolume);
            }
        }

        #endregion

        #region Sound Effects

        /// <summary>PlayAudio( soundName,volume,"loop" )...
        /// PlayAudio(soundName,1.0) plays soundName once at 100% volume...
        /// if third parameter was word "loop" it would loop "volume" is a
        /// number from 0.0 to 1.0 "loop" is the word "loop" (or "true"),
        /// which tells the sound to loop over and over</summary>
        public void PlayAudio(string soundName, bool loop = false, float volume = 1)
        {
            EventReference audioClip;
            if (!audioDict.ContainsKey(soundName) || (audioClip = audioDict[soundName]).IsNull)
            {
                Debug.Log("Audio event reference not found", this);
                return;
            }

            if (volume <= 0.01f)
            {
                Debug.LogWarningFormat(this, "VN Manager is playing sound {0} at very low volume ({1}), just so you know", soundName, volume);
            }

            EventInstance newAudioSource = RuntimeManager.CreateInstance(audioClip);
            float v;
            newAudioSource.getVolume(out v);
            newAudioSource.setVolume(v * volume);
            newAudioSource.start();
            audioInstances.Add(soundName, newAudioSource);

            // if it doesn't loop, let's set a max lifetime for this sound
            if (!loop)
            {
                newAudioSource.getDescription(out EventDescription d);
                d.getLength(out int length);
                //StartCoroutine(SetDestroyTime(newAudioSource, length));
            }
        }

        /// <summary>stops sound playback based on sound name, whether it's
        /// looping or not</summary>
        public void StopAudio(string soundName)
        {
            if (audioInstances.ContainsKey(soundName))
            {
                if (audioInstances[soundName].isValid())
                    audioInstances[soundName].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                audioInstances.Remove(soundName);
            }
            else
            {
                Debug.LogWarningFormat(this, "VN Manager tried to <<StopAudio {0}>> but couldn't find any sound \"{0}\" currently playing. Double-check the name, or maybe it already stopped.", soundName);
                return;
            }
        }

        /// <summary>stops all currently playing sounds, doesn't actually
        /// take any parameters</summary>
        public void StopAudioAll()
        {
            List<string> audios = audioInstances.Keys.ToList();
            foreach (string audio in audios)
            {
                StopAudio(audio);
            }
        }

        #endregion







        public void PlayOneShot(string soundName)
        {
            EventReference audioClip;
            if (audioDict.ContainsKey(soundName) && !(audioClip = audioDict[soundName]).IsNull)
            {
                PlayOneShot(audioClip);
                return;
            }
            else Debug.Log("Audio event reference not found", this);
        }

        public void PlayOneShot(EventReference sound)
        {
            RuntimeManager.PlayOneShot(sound);
        }

        public void PlayOneShot(EventReference sound, Vector3 worldPos)
        {
            RuntimeManager.PlayOneShot(sound, worldPos);
        }
    }
}
