using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI.Styles;
using Mystie.Dressup;
using Mystie.UI;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

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
        public event Action<LevelStageType> onStageComplete;

        #endregion

        public EpisodeManager episodeManager { get; private set; }
        [field: SerializeField] public EpisodeScriptable episode { get; private set; }
        public ContestantData contestant { get; private set; }

        [field: SerializeField] public DressupUIManager uiManager { get; private set; }
        [field: SerializeField] public DressupManager dressup { get; private set; }
        [field: SerializeField] public InventoryManager inventory { get; private set; }
        [field: SerializeField] public DossierManager dossier { get; private set; }
        [field: SerializeField] public EmailManager emailManager { get; private set; }

        public bool IsBossReactionPositive
        {
            get => dressup.CheckStyleRule() && dressup.CheckTrending();
        }

        public bool episodeIndexOverride;
        [ShowIf("episodeIndexOverride")]
        public int episodeIndex;

        private int stageIndex;
        public bool stagesOverride;
        [ShowIf("stagesOverride")]
        public List<LevelStageType> stages = new List<LevelStageType>();

        public LevelStageType CurrentStage
        {
            get => stageIndex < stages.Count ? stages[stageIndex] : stages[stages.Count - 1];
        }

        public void Awake()
        {
            Debug.Log("Level manager");
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Debug.Log("Level manager 2");

            episodeManager = EpisodeManager.Instance;
            if (episodeIndexOverride) episodeManager.SetEpisodeIndex(episodeIndex);
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
            if (!stagesOverride) stages = episode.stages;

            emailManager = new EmailManager();

            dressup.Initialize(contestant);
            inventory.Initialize(episode);
            emailManager.Initialize(episode);
            uiManager.Initialize(this);
        }

        private void Initialize()
        {
            Debug.Log("Level manager start");

            stageIndex = 0;
            onStageSet?.Invoke(CurrentStage);
        }

        private void OnDestroy()
        {
            emailManager.Clean();
        }

        public void CompleteStage(LevelStageType stage)
        {
            if (CurrentStage == stage)
            {
                onStageComplete?.Invoke(CurrentStage);

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

            inventory.GainMoney(GetRevenue());
            inventory.SaveInventory();

            SaveDataManager.SaveGameData();
            //GameManager.Instance.LoadMainMenu();
        }

        public int GetRevenue()
        {
            int positiveReactions = 0;
            int revenue = episode.revenueSocialMediaNeutral;
            foreach (ViewerGroupScriptable group in episode.socialMediaComments.viewerGroups)
            {
                switch (group.GetReaction(dressup))
                {
                    case Reaction.Positive:
                        positiveReactions += 1;
                        break;
                    case Reaction.Negative:
                        positiveReactions -= 1;
                        break;
                    case Reaction.Neutral:
                        break;
                    default:
                        break;
                }
            }

            if (positiveReactions >= 3) revenue = episode.revenueSocialMediaPositive;
            else if (positiveReactions <= 3) revenue = episode.revenueSocialMediaNegative;

            bool bossPositiveReaction = IsBossReactionPositive;
            int bossBonus = bossPositiveReaction ?
                Math.Clamp(positiveReactions * episode.bossBonusPerGroup, 0, episode.bossBonusMax)
                : 0;

            Debug.Log($"Total revenue: {revenue}$"
            + $"\nSocial media score: {positiveReactions}."
            + $"\nBoss reaction positive: {bossPositiveReaction} ({bossBonus}$).");

            return revenue;
        }
    }
}
