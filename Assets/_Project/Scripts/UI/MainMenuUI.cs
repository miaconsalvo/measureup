using Mystie.Core;
using Mystie.UI.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mystie.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        public EpisodeManager episodeManager { get; private set; }
        [SerializeField] private Button newGameBtn;
        [SerializeField] private Button continueBtn;

        public IEnumerator Start()
        {
            SaveManager.LoadSaveFiles();
            bool hasSave = SaveManager.HasSave();
            if (continueBtn != null) continueBtn.gameObject.SetActive(hasSave);

            // Wait for the localization system to initialize, loading Locales, preloading etc.
            yield return LocalizationSettings.InitializationOperation;
            GameManager.Instance.gameSettings.LoadLocale();
            //stringFormatter = LocalizationSettings.StringDatabase.SmartFormatter;
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
