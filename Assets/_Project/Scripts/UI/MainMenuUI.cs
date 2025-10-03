using Mystie.Core;
using Mystie.UI.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mystie.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        public EpisodeManager episodeManager { get; private set; }
        [SerializeField] private Button newGameBtn;
        [SerializeField] private Button continueBtn;

        private void OnEnable()
        {
            if (newGameBtn != null) newGameBtn.onClick.AddListener(NewGame);
            if (continueBtn != null) continueBtn.onClick.AddListener(Continue);
        }

        private void OnDisable()
        {
            if (newGameBtn != null) newGameBtn.onClick.RemoveListener(NewGame);
            if (continueBtn != null) continueBtn.onClick.RemoveListener(Continue);
        }

        private void NewGame()
        {
            EpisodeManager.Instance.LoadEpisode(0);
            //SceneTransitioner.Instance.LoadScene(startScene, transitionMode);
        }

        private void Continue()
        {
            EpisodeManager.Instance.LoadNextEpisode();
            //SceneTransitioner.Instance.LoadScene(startScene, transitionMode);
        }
    }
}
