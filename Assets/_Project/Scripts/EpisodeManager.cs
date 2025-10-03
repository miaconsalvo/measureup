using System.Collections.Generic;
using Mystie.UI.Transition;
using NaughtyAttributes;
using UnityEngine;

namespace Mystie
{
    public class EpisodeManager : MonoBehaviour
    {
        [Scene, SerializeField] private string episodeScene;
        [SerializeField, Scene] private string onbardingScene;
        [SerializeField, Scene] private string endScene;
        [field: SerializeField] public int index { get; private set; } = 0;
        [SerializeField] private SceneTransitionMode transitionMode;
        [field: SerializeField] public List<EpisodeScriptable> episodes { get; private set; }

        private static EpisodeManager instance;

        public static EpisodeManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindAnyObjectByType<EpisodeManager>();
                return instance;
            }
        }

        public void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        public void LoadNextEpisode()
        {
            LoadEpisode(index);
        }

        public void LoadEpisode(int i)
        {
            if (i < 0)
            {
                Debug.LogError($"Episode index {i} is invalid");
                return;
            }

            index = i;
            if (i == 0) SceneTransitioner.Instance.LoadScene(onbardingScene);
            else if (i < episodes.Count) SceneTransitioner.Instance.LoadScene(episodeScene);
            else SceneTransitioner.Instance.LoadScene(endScene);
        }

        public void CompleteEpisode()
        {
            if (index < episodes.Count)
                index += 1;
        }
    }
}
