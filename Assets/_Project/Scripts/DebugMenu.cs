using System;
using System.Collections.Generic;
using Mystie.UI;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.MystEditor
{
    public class DebugMenu : MonoBehaviour
    {
        [field: SerializeField] public CanvasGroup canvas { get; private set; }
        [field: SerializeField] public Button debugMenuButton { get; private set; }
        [field: SerializeField] public Button debugButtonPrefab { get; private set; }
        private bool open;

        public static DebugMenu instance;

        public RectTransform levelAnchor;
        public List<DebugLevelButton> levels;

        public void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            if (!Debug.isDebugBuild)
            {
                debugMenuButton.gameObject.SetActive(false);
                return;
            }

            foreach (DebugLevelButton level in levels)
            {
                level.Init(Instantiate(debugButtonPrefab, levelAnchor));
            }

            debugMenuButton.onClick.AddListener(ToggleDebugMenu);
        }

        public void OnDestroy()
        {
            debugMenuButton.onClick.RemoveListener(ToggleDebugMenu);

            foreach (DebugLevelButton level in levels)
            {
                level.Clean();
            }
        }

        public void ToggleDebugMenu()
        {
            open = !open;
            if (open)
            {
                canvas.alpha = 1f;
                canvas.blocksRaycasts = true;
            }
            else
            {
                canvas.alpha = 0f;
                canvas.blocksRaycasts = false;
            }
        }

        [Serializable]
        public class DebugLevelButton
        {
            public string name;
            private Button button;
            public int levelIndex = 0;

            public void Init(Button button)
            {
                if (button == null) return;
                this.button = button;
                button.onClick.AddListener(Load);
                button.GetComponentInChildren<TextMeshProUGUI>().text = name;
            }

            public void Clean()
            {
                if (button == null) return;
                button.onClick.RemoveListener(Load);
            }

            public void Load()
            {
                EpisodeManager.Instance.LoadEpisode(levelIndex);
            }
        }
    }
}
