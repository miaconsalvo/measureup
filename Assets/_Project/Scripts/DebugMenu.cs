using System;
using System.Collections.Generic;
using Mystie.Dressup;
using Mystie.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace Mystie.MystEditor
{
    public class DebugMenu : MonoBehaviour
    {
        public static DebugMenu instance;

        [field: SerializeField] public CanvasGroup canvas { get; private set; }
        [field: SerializeField] public Button debugMenuButton { get; private set; }
        [field: SerializeField] public Button debugButtonPrefab { get; private set; }
        private bool open;

        public RectTransform levelAnchor;
        public List<DebugLevelButton> levels;

        [Space]

        public RectTransform contestantsAnchor;
        public List<ContestantData> contestants;
        [field: SerializeField] public ContestantDebugUI debugContestantPrefab { get; private set; }

        private List<DebugContestant> contestantsDebug;

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

            contestantsDebug = new List<DebugContestant>();
            foreach (ContestantData c in contestants)
            {
                ContestantDebugUI debug = Instantiate(debugContestantPrefab.gameObject, contestantsAnchor).GetComponent<ContestantDebugUI>();
                DebugContestant contestant = new DebugContestant(c, debug);
                contestantsDebug.Add(contestant);
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

        [Button]
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

        public class DebugContestant
        {
            public ContestantData data;
            private ContestantDebugUI debug;

            public DebugContestant(ContestantData data, ContestantDebugUI debug)
            {
                this.data = data;
                this.debug = debug;
                debug.Set(data);
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
