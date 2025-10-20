using System;
using System.Collections;
using System.Collections.Generic;
using Mystie.Dressup;
using Mystie.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Mystie.Core
{
    public class LevelManager : MonoBehaviour
    {
        #region Singleton

        private static LevelManager instance;
        public static LevelManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<LevelManager>();
                }

                return instance;
            }
        }

        #endregion

        #region Events

        public event Action<LevelStageType> onStageSet;

        #endregion

        public EpisodeManager episodeManager { get; private set; }
        [field: SerializeField] public EpisodeScriptable episode { get; private set; }
        public ContestantData contestant { get; private set; }

        [field: SerializeField] public DressupUIManager uiManager { get; private set; }
        [field: SerializeField] public DressupManager dressup { get; private set; }
        [field: SerializeField] public InventoryManager inventory { get; private set; }
        [field: SerializeField] public DossierManager dossier { get; private set; }

        private int stageIndex;
        public List<LevelStageType> stages = new List<LevelStageType>();
        public LevelStageType CurrentStage
        {
            get => stageIndex < stages.Count ? stages[stageIndex] : stages[stages.Count - 1];
        }

        public void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            episodeManager = EpisodeManager.Instance;
            episode = episodeManager.episodes[episodeManager.index];
            InitializeComponents();
        }

        public IEnumerator Start()
        {
            yield return null;
            Initialize();
        }

        public void InitializeComponents()
        {
            contestant = episode.contestant;

            uiManager = DressupUIManager.Instance;

            dressup.Initialize(contestant);
            inventory.Initialize(episode);
            uiManager.Initialize(this);
        }

        private void Initialize()
        {
            Debug.Log("Level manager start");

            stageIndex = 0;
            onStageSet?.Invoke(CurrentStage);
        }

        public void CompleteStage(LevelStageType stage)
        {
            if (CurrentStage == stage)
            {
                stageIndex++;

                if (stageIndex >= stages.Count)
                {
                    OnLevelComplete();
                }
                else
                {
                    Debug.Log("Stage set " + CurrentStage);
                    onStageSet?.Invoke(CurrentStage);
                }
            }
        }

        public void OnLevelComplete()
        {
            Debug.Log("Level Complete!");
            episodeManager.CompleteEpisode();
            episodeManager.LoadCurrentEpisode();
            SaveDataManager.SaveEpisodeIndex(episodeManager.index);

            inventory.SaveInventory();

            SaveDataManager.SaveGameData();
            //GameManager.Instance.LoadMainMenu();
        }
    }
}
