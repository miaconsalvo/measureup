using System;
using System.Collections;
using System.Collections.Generic;
using Mystie.Dressup;
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
                    instance = FindObjectOfType<LevelManager>();

                return instance;
            }
        }

        #endregion

        #region Events

        public event Action<LevelStageType> onStageSet;

        #endregion

        [field: SerializeField] public EpisodeScriptable episode { get; private set; }
        [field: SerializeField] public DossierManager dossier { get; private set; }
        [field: SerializeField] public DressupManager dressup { get; private set; }

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

            dossier = FindObjectOfType<DossierManager>();
            dossier.SetEpisode(episode);
        }

        private void Start()
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
                    Debug.Log("Level Complete!");
                }
                else
                {
                    Debug.Log("Stage set " + CurrentStage);
                    onStageSet?.Invoke(CurrentStage);
                }
            }
        }
    }
}
