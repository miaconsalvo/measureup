using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mystie
{
    public class PlaySFX : StateMachineBehaviour
    {
        [SerializeField] private List<SoundEffect> soundEffects = new List<SoundEffect>();

        private int i = 0;
        private int loop = 0;
        private float duration;

        private void OnValidate()
        {
            soundEffects.Sort((x, y) => x.time.CompareTo(y.time));
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            i = 0;
            loop = 0;
            duration = stateInfo.length;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime > (loop + 1))
            {
                i = 0;
                loop++;
            }

            if (i < soundEffects.Count && CurrentTime(stateInfo.normalizedTime) >= soundEffects[i].time
                && (loop == 0 || soundEffects[i].loop))
            {
                //Debug.Log(i + ": " + CurrentTime(stateInfo.normalizedTime));
                soundEffects[i].Play(animator.transform);
                i++;
            }
        }

        private float CurrentTime(float normalizedTime)
        {
            return (normalizedTime - loop) * duration;
        }

        [System.Serializable]
        public class SoundEffect
        {
            [SerializeField] private EventReference sfx;
            public float time = 0f;
            public bool loop;

            public void Play(Transform t)
            {
                RuntimeManager.PlayOneShot(sfx, t.position);
            }
        }
    }
}
