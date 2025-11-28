using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI.Styles;
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

        [field: SerializeField] public int revenueSocialMediaPositive { get; private set; } = 1000;
        [field: SerializeField] public int revenueSocialMediaNeutral { get; private set; } = 750;
        [field: SerializeField] public int revenueSocialMediaNegative { get; private set; } = 250;

        [field: Space]

        [field: SerializeField] public int bossBonusPerGroup { get; private set; } = 150;
        [field: SerializeField] public int bossBonusMax { get; private set; } = 600;

        public bool IsBossReactionPositive
        {
            get => dressup.CheckStyleRule() && dressup.CheckTrending();
        }

        private int stageIndex;
        public bool stagesOverride;
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
            int revenue = revenueSocialMediaNeutral;
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

            if (positiveReactions >= 3) revenue = revenueSocialMediaPositive;
            else if (positiveReactions <= 3) revenue = revenueSocialMediaNegative;

            bool bossPositiveReaction = IsBossReactionPositive;
            int bossBonus = bossPositiveReaction ?
                Math.Clamp(positiveReactions * bossBonusPerGroup, 0, bossBonusMax)
                : 0;

            Debug.Log($"Total revenue: {revenue}$"
            + $"\nSocial media score: {positiveReactions}."
            + $"\nBoss reaction positive: {bossPositiveReaction} ({bossBonus}$).");

            return revenue;
        }
    }
}
