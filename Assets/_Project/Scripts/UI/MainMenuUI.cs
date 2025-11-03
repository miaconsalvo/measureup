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

        public void Start()
        {
            SaveManager.LoadSaveFiles();
            bool hasSave = SaveManager.HasSave();
            if (continueBtn != null) continueBtn.gameObject.SetActive(hasSave);
        }

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
            SaveManager.NewGame();
            //SceneTransitioner.Instance.LoadScene(startScene, transitionMode);
        }

        private void Continue()
        {
            SaveManager.LoadGame();
            //SceneTransitioner.Instance.LoadScene(startScene, transitionMode);
        }
    }
}
